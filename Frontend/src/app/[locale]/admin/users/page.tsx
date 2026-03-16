"use client";

import { useEffect, useState } from "react";
import { getAccessToken } from "@/lib/auth-client";
import { addUserBalance } from "@/actions/adminActions";

interface User {
    id: string;
    email: string;
    username: string;
    displayName: string;
    level: number;
    exp: number;
    goldBalance: number;
    diamondBalance: number;
    role: string; // Thêm role
    isLocked: boolean;
    createdAt: string;
}

export default function AdminUsersPage() {
    const [users, setUsers] = useState<User[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState("");
    const [loading, setLoading] = useState(true);

    // State cho Modal nạp tiền
    const [isAddBalanceOpen, setIsAddBalanceOpen] = useState(false);
    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const [addAmount, setAddAmount] = useState(100);
    const [addCurrency, setAddCurrency] = useState("gold");
    const [addReason, setAddReason] = useState("");
    const [actionLoading, setActionLoading] = useState(false);

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            const token = getAccessToken();
            const res = await fetch(`${baseUrl}/admin/users?page=${page}&pageSize=10&searchTerm=${encodeURIComponent(searchTerm)}`, {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                }
            });
            if (!res.ok) {
                throw new Error("Failed to fetch");
            }
            const data = await res.json();
            setUsers(data.users);
            setTotalCount(data.totalCount);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, [page, searchTerm]);

    const handleToggleLock = async (userId: string, isCurrentlyLocked: boolean) => {
        if (!confirm(`Bạn có chắc muốn ${isCurrentlyLocked ? 'Mở khóa' : 'Khóa'} người dùng này?`)) return;

        try {
            const action = isCurrentlyLocked ? "unlock" : "lock";
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            const token = getAccessToken();
            const res = await fetch(`${baseUrl}/admin/users/toggle-lock`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify({ userId, action })
            });
            if (res.ok) {
                await fetchUsers(); // Tải lại danh sách
            } else {
                alert("Có lỗi xảy ra khi thực hiện hành động này.");
            }
        } catch {
            alert("Lỗi kết nối.");
        }
    };

    const handleAddBalance = async () => {
        if (!selectedUser) return;
        if (addAmount <= 0) {
            alert("Số tiền phải lớn hơn 0");
            return;
        }

        setActionLoading(true);
        try {
            const success = await addUserBalance(selectedUser.id, addCurrency, addAmount, addReason || `Admin manual topup ${addCurrency}`);
            if (success) {
                setIsAddBalanceOpen(false);
                setAddReason("");
                await fetchUsers();
            } else {
                alert("Lỗi khi cộng tiền.");
            }
        } catch (err) {
            console.error(err);
            alert("Lỗi hệ thống.");
        } finally {
            setActionLoading(false);
        }
    };

    return (
        <div className="space-y-6">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <h1 className="text-3xl font-extrabold text-[#DFF2CB]">Quản Lý Người Dùng</h1>
                <div className="relative">
                    <input
                        type="text"
                        placeholder="Tìm email, username..."
                        value={searchTerm}
                        onChange={(e) => {
                            setSearchTerm(e.target.value);
                            setPage(1); // Reset page về 1 khi tìm kiếm
                        }}
                        className="w-full md:w-64 bg-[#1A1F2B] border border-[#2D3748] rounded-lg pl-4 pr-10 py-2 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors"
                    />
                </div>
            </div>

            <div className="bg-[#1A1F2B] border border-[#2D3748] rounded-2xl overflow-hidden shadow-2xl">
                <div className="overflow-x-auto">
                    <table className="w-full text-left text-sm text-gray-300">
                        <thead className="text-xs uppercase bg-[#0F1219] text-[#DFF2CB] border-b border-[#2D3748]">
                            <tr>
                                <th className="px-6 py-4">Tài Khoản</th>
                                <th className="px-6 py-4">Cấp Độ / EXP</th>
                                <th className="px-6 py-4">Số Dư (G/D)</th>
                                <th className="px-6 py-4">Vai Trò</th>
                                <th className="px-6 py-4">Ngày Tham Gia</th>
                                <th className="px-6 py-4 text-center">Trạng Thái</th>
                                <th className="px-6 py-4 text-center">Hành Động</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading ? (
                                <tr>
                                    <td colSpan={7} className="px-6 py-8 text-center text-gray-500">Đang tải dữ liệu...</td>
                                </tr>
                            ) : users.length === 0 ? (
                                <tr>
                                    <td colSpan={7} className="px-6 py-8 text-center text-gray-500">Không tìm thấy người dùng nào.</td>
                                </tr>
                            ) : (
                                users.map((u) => (
                                    <tr key={u.id} className="border-b border-[#2D3748] hover:bg-[#2D3748]/30 transition-colors">
                                        <td className="px-6 py-4">
                                            <div className="font-semibold text-white">{u.displayName}</div>
                                            <div className="text-xs text-gray-400">{u.email}</div>
                                        </td>
                                        <td className="px-6 py-4">
                                            Lv {u.level} <span className="text-xs text-gray-500">({u.exp} XP)</span>
                                        </td>
                                        <td className="px-6 py-4 font-mono text-sm">
                                            <div className="text-yellow-400">G: {u.goldBalance}</div>
                                            <div className="text-[#DFF2CB]">D: {u.diamondBalance}</div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <span className={`px-2.5 py-1 rounded-lg text-xs font-bold uppercase ${u.role === "admin"
                                                    ? "bg-purple-500/20 text-purple-400 border border-purple-500/30"
                                                    : u.role === "tarot_reader"
                                                        ? "bg-blue-500/20 text-blue-400 border border-blue-500/30"
                                                        : "bg-gray-500/20 text-gray-400 border border-gray-500/30"
                                                }`}>
                                                {u.role === "admin" ? "Admin" : u.role === "tarot_reader" ? "Reader" : "User"}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4">{new Date(u.createdAt).toLocaleDateString("vi-VN")}</td>
                                        <td className="px-6 py-4 text-center">
                                            <span className={`px-2.5 py-1 rounded-full text-xs font-semibold ${u.isLocked ? "bg-red-500/20 text-red-400" : "bg-green-500/20 text-green-400"}`}>
                                                {u.isLocked ? "Bị Khóa" : "Hoạt Động"}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-center space-x-2">
                                            <button
                                                onClick={() => {
                                                    setSelectedUser(u);
                                                    setIsAddBalanceOpen(true);
                                                }}
                                                className="text-xs px-3 py-1.5 rounded-lg border border-[#DFF2CB] text-[#DFF2CB] hover:bg-[#DFF2CB] hover:text-[#0F1219] transition-all"
                                                title="Cộng tiền"
                                            >
                                                + Tiền
                                            </button>
                                            <button
                                                onClick={() => handleToggleLock(u.id, u.isLocked)}
                                                className={`text-xs px-3 py-1.5 rounded-lg border transition-colors ${u.isLocked
                                                        ? "border-green-500 text-green-400 hover:bg-green-500 hover:text-white"
                                                        : "border-red-500 text-red-500 hover:bg-red-500 hover:text-white"
                                                    }`}
                                            >
                                                {u.isLocked ? "Mở Khóa" : "Khóa"}
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>

                {/* Pagination */}
                <div className="px-6 py-4 bg-[#0F1219] flex justify-between items-center border-t border-[#2D3748]">
                    <span className="text-sm text-gray-400">
                        Tổng cộng: <strong className="text-white">{totalCount}</strong> người dùng
                    </span>
                    <div className="flex gap-2">
                        <button
                            onClick={() => setPage(p => Math.max(1, p - 1))}
                            disabled={page === 1}
                            className="px-3 py-1 bg-[#1A1F2B] hover:bg-[#2D3748] rounded border border-[#2D3748] disabled:opacity-50 transition-colors"
                        >
                            Trước
                        </button>
                        <span className="px-3 py-1 text-sm font-semibold">{page}</span>
                        <button
                            onClick={() => setPage(p => p + 1)}
                            disabled={page * 10 >= totalCount}
                            className="px-3 py-1 bg-[#1A1F2B] hover:bg-[#2D3748] rounded border border-[#2D3748] disabled:opacity-50 transition-colors"
                        >
                            Sau
                        </button>
                    </div>
                </div>
            </div>

            {/* Modal cộng tiền */}
            {isAddBalanceOpen && (
                <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4">
                    <div className="bg-[#1A1F2B] border border-[#2D3748] rounded-2xl w-full max-w-md overflow-hidden shadow-2xl animate-in fade-in zoom-in duration-200">
                        <div className="p-6 border-b border-[#2D3748] flex justify-between items-center bg-[#0F1219]">
                            <h2 className="text-xl font-bold text-[#DFF2CB]">Cộng Tiền Thủ Công</h2>
                            <button onClick={() => setIsAddBalanceOpen(false)} className="text-gray-400 hover:text-white transition-colors">
                                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" /></svg>
                            </button>
                        </div>
                        <div className="p-6 space-y-4">
                            <div>
                                <label className="block text-xs uppercase text-gray-500 mb-2">Người nhận</label>
                                <div className="text-white font-semibold bg-[#2D3748]/30 px-3 py-2 rounded-lg border border-[#2D3748]">
                                    {selectedUser?.displayName} ({selectedUser?.email})
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-3">
                                <label className={`flex items-center justify-center gap-2 p-3 rounded-xl border-2 transition-all cursor-pointer ${addCurrency === "gold" ? "border-yellow-500 bg-yellow-500/10 text-yellow-500" : "border-[#2D3748] text-gray-400 hover:border-gray-500"}`}>
                                    <input type="radio" value="gold" checked={addCurrency === "gold"} onChange={() => setAddCurrency("gold")} className="hidden" />
                                    <span className="font-bold">GOLD</span>
                                </label>
                                <label className={`flex items-center justify-center gap-2 p-3 rounded-xl border-2 transition-all cursor-pointer ${addCurrency === "diamond" ? "border-[#DFF2CB] bg-[#DFF2CB]/10 text-[#DFF2CB]" : "border-[#2D3748] text-gray-400 hover:border-gray-500"}`}>
                                    <input type="radio" value="diamond" checked={addCurrency === "diamond"} onChange={() => setAddCurrency("diamond")} className="hidden" />
                                    <span className="font-bold">DIAMOND</span>
                                </label>
                            </div>

                            <div>
                                <label className="block text-xs uppercase text-gray-500 mb-2">Số lượng cộng</label>
                                <input
                                    type="number"
                                    value={addAmount}
                                    onChange={(e) => setAddAmount(Number(e.target.value))}
                                    className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-2.5 text-white focus:outline-none focus:border-[#DFF2CB] font-mono text-lg"
                                />
                            </div>

                            <div>
                                <label className="block text-xs uppercase text-gray-500 mb-2">Lý do (Lưu nhật ký)</label>
                                <textarea
                                    value={addReason}
                                    onChange={(e) => setAddReason(e.target.value)}
                                    placeholder="Ví dụ: Đền bù lỗi hệ thống, Thưởng sự kiện..."
                                    className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-2 text-sm text-white focus:outline-none focus:border-[#DFF2CB] h-20 resize-none"
                                />
                            </div>

                            <div className="pt-4 flex gap-3">
                                <button
                                    onClick={() => setIsAddBalanceOpen(false)}
                                    className="flex-1 px-4 py-3 rounded-xl bg-gray-800 text-gray-300 font-bold hover:bg-gray-700 transition-colors"
                                >
                                    Hủy
                                </button>
                                <button
                                    onClick={handleAddBalance}
                                    disabled={actionLoading}
                                    className="flex-1 px-4 py-3 rounded-xl bg-[#DFF2CB] text-[#0F1219] font-extrabold hover:bg-white transition-all disabled:opacity-50"
                                >
                                    {actionLoading ? "Đang xử lý..." : "Xác Nhận"}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
