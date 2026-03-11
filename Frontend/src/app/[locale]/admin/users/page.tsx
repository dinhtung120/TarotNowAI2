"use client";

import { useEffect, useState } from "react";

interface User {
    id: string;
    email: string;
    username: string;
    displayName: string;
    level: number;
    exp: number;
    diamondBalance: number;
    isLocked: boolean;
    createdAt: string;
}

export default function AdminUsersPage() {
    const [users, setUsers] = useState<User[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState("");
    const [loading, setLoading] = useState(true);
    const [errorStatus, setErrorStatus] = useState<number | null>(null);

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const res = await fetch(`/api/v1/admin/users?page=${page}&pageSize=10&searchTerm=${encodeURIComponent(searchTerm)}`, {
                headers: {
                    "Content-Type": "application/json"
                }
            });
            if (!res.ok) {
                if (res.status === 403 || res.status === 401) {
                    setErrorStatus(res.status);
                }
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
            const res = await fetch("/api/v1/admin/users/toggle-lock", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
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

    if (errorStatus === 401 || errorStatus === 403) {
        return (
            <div className="flex flex-col items-center justify-center h-full text-red-400 space-y-4">
                <h1 className="text-4xl font-bold">403 Forbidden</h1>
                <p>Bạn không có quyền Admin để truy cập trang này.</p>
            </div>
        );
    }

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
                                <th className="px-6 py-4">Số Dư (Diamond)</th>
                                <th className="px-6 py-4">Ngày Tham Gia</th>
                                <th className="px-6 py-4 text-center">Trạng Thái</th>
                                <th className="px-6 py-4 text-center">Hành Động</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading ? (
                                <tr>
                                    <td colSpan={6} className="px-6 py-8 text-center text-gray-500">Đang tải dữ liệu...</td>
                                </tr>
                            ) : users.length === 0 ? (
                                <tr>
                                    <td colSpan={6} className="px-6 py-8 text-center text-gray-500">Không tìm thấy người dùng nào.</td>
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
                                        <td className="px-6 py-4 font-mono text-[#DFF2CB]">{u.diamondBalance}</td>
                                        <td className="px-6 py-4">{new Date(u.createdAt).toLocaleDateString("vi-VN")}</td>
                                        <td className="px-6 py-4 text-center">
                                            <span className={`px-2.5 py-1 rounded-full text-xs font-semibold ${u.isLocked ? "bg-red-500/20 text-red-400" : "bg-green-500/20 text-green-400"}`}>
                                                {u.isLocked ? "Bị Khóa" : "Hoạt Động"}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-center">
                                            <button
                                                onClick={() => handleToggleLock(u.id, u.isLocked)}
                                                className={`text-xs px-4 py-1.5 rounded-lg border transition-colors ${u.isLocked
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
        </div>
    );
}
