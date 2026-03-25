/*
 * ===================================================================
 * FILE: page.tsx (Admin Users Management)
 * BỐI CẢNH (CONTEXT):
 *   Phân hệ User dành cho Admin.
 *   Bao gồm: Xem danh sách user, Mở giao diện "Edit User Modal" thay đổi chức vụ, 
 *   số dư Kim Cương / Vàng, và đóng/mở tài khoản chung bộ 1 chạm.
 * 
 * KIẾN TRÚC UI:
 *   Áp dụng mô hình Glassmorphism tối đa với Edit Modal. Data mutation
 *   được tính bằng hiệu số (Delta) để an toàn sổ cái.
 * ===================================================================
 */
"use client";

import { Users, Search, Lock, Gem, Coins,
	Activity,
	Star,
	Mail,
	Loader2,
	X,
	Edit2,
	UserPlus
} from "lucide-react";
import { SectionHeader, GlassCard, Button, Input, TableStates, StepPagination } from "@/shared/components/ui";
import { useAdminUsers } from "@/features/admin/users/application/useAdminUsers";

export default function AdminUsersPage() {
	const {
		t,
		locale,
		users,
		totalCount,
		page,
		setPage,
			searchTerm,
			setSearchTerm,
			loading,
			addModal,
			addForm,
			setAddForm,
			editModal,
			editForm,
			setEditForm,
			actionLoading,
			createLoading,
			handleOpenAdd,
			closeAddModal,
			handleCreateUser,
			handleOpenEdit,
			closeEditModal,
			handleSaveUser,
		} = useAdminUsers();

	return (
			<div className="space-y-8 pb-20 animate-in fade-in duration-700">

				{/* Add User Modal */}
				{addModal.isOpen && (
					<div className="fixed inset-0 z-[160] flex items-center justify-center p-4 md:p-6 animate-in fade-in duration-300">
						<div className="absolute inset-0 tn-overlay-strong" onClick={closeAddModal} />
						<div className="relative z-10 w-full max-w-xl tn-panel rounded-[3rem] overflow-hidden shadow-[0_0_100px_var(--c-168-85-247-15)] animate-in zoom-in-95 duration-300">
							<div className="p-8 border-b tn-border-soft tn-surface flex items-center justify-between">
								<div className="flex items-center gap-4">
									<div className="w-12 h-12 rounded-2xl bg-[var(--info)]/10 border border-[var(--info)]/20 flex items-center justify-center shadow-inner">
										<UserPlus className="w-6 h-6 text-[var(--info)]" />
									</div>
									<div className="text-left">
										<h2 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">{t("users.add_user.title")}</h2>
										<p className="text-[9px] font-black text-[var(--text-tertiary)] uppercase tracking-widest">{t("users.add_user.subtitle")}</p>
									</div>
								</div>
								<button
									onClick={closeAddModal}
									className="w-10 h-10 rounded-full tn-surface flex items-center justify-center text-[var(--text-secondary)] hover:bg-[var(--danger)] hover:tn-text-primary transition-all shadow-xl border border-transparent"
								>
									<X className="w-5 h-5" />
								</button>
							</div>

							<div className="p-8 space-y-5">
								<div className="grid grid-cols-1 md:grid-cols-2 gap-5">
									<div className="space-y-2">
										<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">
											{t("users.add_user.email_label")}
										</label>
										<input
											type="email"
											value={addForm.email}
											onChange={(e) => setAddForm((prev) => ({ ...prev, email: e.target.value }))}
											placeholder={t("users.add_user.email_placeholder")}
											className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold shadow-inner"
										/>
									</div>
									<div className="space-y-2">
										<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">
											{t("users.add_user.username_label")}
										</label>
										<input
											type="text"
											value={addForm.username}
											onChange={(e) => setAddForm((prev) => ({ ...prev, username: e.target.value }))}
											placeholder={t("users.add_user.username_placeholder")}
											className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold shadow-inner"
										/>
									</div>
								</div>

								<div className="grid grid-cols-1 md:grid-cols-2 gap-5">
									<div className="space-y-2">
										<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">
											{t("users.add_user.display_name_label")}
										</label>
										<input
											type="text"
											value={addForm.displayName}
											onChange={(e) => setAddForm((prev) => ({ ...prev, displayName: e.target.value }))}
											placeholder={t("users.add_user.display_name_placeholder")}
											className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold shadow-inner"
										/>
									</div>
									<div className="space-y-2">
										<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">
											{t("users.add_user.password_label")}
										</label>
										<input
											type="password"
											value={addForm.password}
											onChange={(e) => setAddForm((prev) => ({ ...prev, password: e.target.value }))}
											placeholder={t("users.add_user.password_placeholder")}
											className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold shadow-inner"
										/>
									</div>
								</div>

								<div className="space-y-2">
									<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">
										{t("users.add_user.role_label")}
									</label>
									<select
										value={addForm.role}
										onChange={(e) => setAddForm((prev) => ({ ...prev, role: e.target.value }))}
										className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold shadow-inner"
									>
										<option value="user">{t("users.roles.user")}</option>
										<option value="tarot_reader">{t("users.roles.tarot_reader")}</option>
										<option value="admin">{t("users.roles.admin")}</option>
									</select>
								</div>
							</div>

							<div className="flex gap-4 p-8 pt-0">
								<Button
									variant="secondary"
									onClick={closeAddModal}
									disabled={createLoading}
									className="flex-1 py-5 shadow-sm"
								>
									{t("users.add_user.cancel")}
								</Button>
								<Button
									variant="primary"
									onClick={handleCreateUser}
									disabled={createLoading}
									className="flex-1 py-5 shadow-[0_0_20px_var(--c-168-85-247-30)] hover:shadow-[0_0_30px_var(--c-168-85-247-50)]"
								>
									{createLoading ? (
										<Loader2 className="w-5 h-5 animate-spin mx-auto" />
									) : (
										<span className="flex items-center justify-center gap-2">
											{t("users.add_user.submit")} <UserPlus className="w-4 h-4 ml-1" />
										</span>
									)}
								</Button>
							</div>
						</div>
					</div>
				)}

				{/* Custom Edit User Modal */}
				{editModal.isOpen && editModal.user && (
				<div className="fixed inset-0 z-[150] flex items-center justify-center p-4 md:p-6 animate-in fade-in duration-300">
					<div className="absolute inset-0 tn-overlay-strong " onClick={closeEditModal} />
					<div className="relative z-10 w-full max-w-lg tn-panel rounded-[3rem] overflow-hidden shadow-[0_0_100px_var(--c-168-85-247-15)] animate-in zoom-in-95 duration-300">
						
						{/* Modal Header */}
						<div className="p-8 border-b tn-border-soft tn-surface flex items-center justify-between">
							<div className="flex items-center gap-4">
								<div className="w-12 h-12 rounded-2xl bg-[var(--warning)]/10 border border-[var(--warning)]/20 flex items-center justify-center shadow-inner">
									<Edit2 className="w-6 h-6 text-[var(--warning)]" />
								</div>
								<div className="text-left">
									<h2 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">Sửa Thông Tin</h2>
									<p className="text-[9px] font-black text-[var(--text-tertiary)] uppercase tracking-widest">{editModal.user.displayName} / {editModal.user.email}</p>
								</div>
							</div>
							<button onClick={closeEditModal}
								className="w-10 h-10 rounded-full tn-surface flex items-center justify-center text-[var(--text-secondary)] hover:bg-[var(--danger)] hover:tn-text-primary transition-all shadow-xl border border-transparent"
							>
								<X className="w-5 h-5" />
							</button>
						</div>

						{/* Modal Body Info Fields */}
						<div className="p-8 space-y-6">
							<div className="grid grid-cols-2 gap-6">
								{/* Field: Role */}
								<div className="space-y-3">
									<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">Chức Vụ Hệ Thống</label>
									<select
										value={editForm.role}
										onChange={(e) => setEditForm(prev => ({ ...prev, role: e.target.value }))}
										className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold italic shadow-inner"
									>
										<option value="user">USER (Khách)</option>
										<option value="tarot_reader">TAROT READER (Thầy)</option>
										<option value="admin">ADMIN (Quản trị)</option>
									</select>
								</div>

								{/* Field: Status */}
								<div className="space-y-3">
									<label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">Trạng Thái (Status)</label>
									<select
										value={editForm.status}
										onChange={(e) => setEditForm(prev => ({ ...prev, status: e.target.value }))}
										className="w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold italic shadow-inner"
									>
										<option value="active">ACTIVE (Đang Hoạt Động)</option>
										<option value="locked">LOCKED (Bị Khóa Cấm)</option>
									</select>
								</div>
							</div>

							{/* Field: Gold & Diamond Balances */}
							<div className="grid grid-cols-2 gap-6 pt-4 border-t tn-border-soft">
								<div className="space-y-3">
									<label className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)] flex items-center gap-1.5 align-middle">
										<Coins className="w-3.5 h-3.5" /> Vàng Hiện Có
									</label>
									<input
										type="number"
										value={editForm.goldBalance}
										onChange={(e) => setEditForm({ ...editForm, goldBalance: Number(e.target.value) })}
										className="w-full tn-field tn-field-accent border-[var(--warning)]/30 text-[var(--warning)] rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter"
									/>
								</div>

								<div className="space-y-3">
									<label className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] flex items-center gap-1.5 align-middle">
										<Gem className="w-3.5 h-3.5" /> KC Hiện Có
									</label>
									<input
										type="number"
										value={editForm.diamondBalance}
										onChange={(e) => setEditForm({ ...editForm, diamondBalance: Number(e.target.value) })}
										className="w-full tn-field tn-field-accent border-[var(--purple-accent)]/30 text-[var(--purple-accent)] rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter"
									/>
								</div>
							</div>

							<div className="p-4 rounded-xl tn-surface-soft border border-[var(--info)]/20 bg-[var(--info)]/5">
								<p className="text-[10px] leading-relaxed font-medium text-[var(--info)]">
									<strong className="uppercase mr-1 block mb-1">Cảnh báo Ledger (Sổ cái):</strong>
									Tuỳ tiện sửa đổi số dư ở đây sẽ tự động kích hoạt tính năng phát sinh hóa đơn chênh lệch (Credit / Debit Manual Adjustment) tại Backend.
								</p>
							</div>
						</div>

						{/* Modal Footer */}
						<div className="flex gap-4 p-8 pt-0">
							<Button
								variant="secondary"
								onClick={closeEditModal}
								className="flex-1 py-5 shadow-sm"
							>
								{t("users.lock_modal.cancel")}
							</Button>
							<Button
								variant="primary"
								onClick={handleSaveUser}
								disabled={actionLoading}
								className="flex-1 py-5 shadow-[0_0_20px_var(--c-168-85-247-30)] hover:shadow-[0_0_30px_var(--c-168-85-247-50)]"
							>
								{actionLoading ? (
									<Loader2 className="w-5 h-5 animate-spin mx-auto" />
								) : (
									<span className="flex items-center justify-center gap-2">
										Xác Nhận & Lưu <Edit2 className="w-4 h-4 ml-1" />
									</span>
								)}
							</Button>
						</div>
					</div>
				</div>
			)}

			{/* Header Area */}
				<div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
				<SectionHeader
					tag={t("users.header.tag")}
					tagIcon={<Users className="w-3 h-3 text-[var(--purple-accent)]" />}
					title={t("users.header.title")}
					subtitle={t("users.header.subtitle", { count: totalCount })}
					className="mb-0 text-left items-start"
				/>

					<div className="flex items-center gap-3 shrink-0">
						<Input
							leftIcon={<Search className="w-4 h-4" />}
							placeholder={t("users.search.placeholder")}
						value={searchTerm}
						onChange={(e) => {
							setSearchTerm(e.target.value);
							setPage(1);
						}}
							className="w-full md:w-80"
						/>
						<Button
							variant="primary"
							onClick={handleOpenAdd}
							className="px-5 py-3 whitespace-nowrap shadow-[0_0_20px_var(--c-168-85-247-25)] hover:shadow-[0_0_30px_var(--c-168-85-247-40)]"
						>
							<span className="inline-flex items-center gap-2">
								<UserPlus className="w-4 h-4" />
								{t("users.add_user.button")}
							</span>
						</Button>
					</div>
				</div>

			{/* Main Table Card */}
			<GlassCard className="!p-0 !rounded-[2.5rem] overflow-hidden">
				<div className="overflow-x-auto custom-scrollbar">
					<table className="w-full text-left">
						<thead>
							<tr className="border-b tn-border-soft tn-surface">
								<th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_account")}</th>
								<th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_rank")}</th>
								<th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_assets")}</th>
								<th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_role")}</th>
								<th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">{t("users.table.heading_status")}</th>
								<th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right">{t("users.table.heading_actions")}</th>
							</tr>
						</thead>
						<tbody className="divide-y divide-white/5">
							<TableStates
								colSpan={6}
								isLoading={loading}
								isEmpty={!loading && users.length === 0}
								loadingLabel={t("users.states.loading")}
								emptyLabel={t("users.states.empty")}
								loadingIcon={<Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)]" />}
								emptyIcon={
									<div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
										<Users className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
									</div>
								}
							/>
							{!loading && users.length > 0 && (
								users.map((u) => (
									<tr key={u.id} className="group/row hover:tn-surface transition-colors">
										<td className="px-8 py-5">
											<div className="flex items-center gap-4">
												<div className="w-10 h-10 rounded-xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--info)]/20 border tn-border flex items-center justify-center tn-text-primary font-black text-sm relative overflow-hidden group-hover/row:scale-110 transition-transform shadow-inner">
													{u.displayName?.charAt(0).toUpperCase() || 'U'}
													<div className="absolute inset-0 tn-surface-strong opacity-0 group-hover/row:opacity-100 transition-opacity" />
												</div>
												<div>
													<div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">{u.displayName}</div>
													<div className="flex items-center gap-1.5 text-[9px] font-bold text-[var(--text-tertiary)]">
														<Mail className="w-2.5 h-2.5" />
														{u.email}
													</div>
												</div>
											</div>
										</td>
										<td className="px-8 py-5">
											<div className="flex items-center gap-2">
												<div className="px-2 py-0.5 rounded-md bg-[var(--warning)]/10 border border-[var(--warning)]/20 text-[9px] font-black text-[var(--warning)] shadow-inner">
													{t("users.row.level", { level: u.level })}
												</div>
												<div className="text-[10px] font-bold text-[var(--text-tertiary)]">{t("users.row.exp", { exp: u.exp })}</div>
											</div>
										</td>
										<td className="px-8 py-5">
											<div className="space-y-1">
												<div className="flex items-center gap-2 text-[11px] font-black tn-text-primary italic">
													<Gem className="w-3 h-3 text-[var(--purple-accent)]" />
													{u.diamondBalance.toLocaleString(locale)}
												</div>
												<div className="flex items-center gap-2 text-[10px] font-bold text-[var(--warning)] italic">
													<Coins className="w-3 h-3" />
													{u.goldBalance.toLocaleString(locale)}
												</div>
											</div>
										</td>
										<td className="px-8 py-5">
											<div className={`
												inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-[9px] font-black uppercase tracking-widest border shadow-inner
												${u.role === "admin" ? "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]" : u.role === "tarot_reader"
													? "bg-[var(--info)]/10 border-[var(--info)]/20 text-[var(--info)]"
													: "tn-surface-strong tn-border text-[var(--text-secondary)]"
												}
											`}>
												<Star className="w-2.5 h-2.5 fill-current" />
												{u.role === "admin" ? t("users.roles.admin") : u.role === "tarot_reader" ? t("users.roles.tarot_reader") : u.role === "user" ? t("users.roles.user") : u.role}
											</div>
										</td>
										<td className="px-8 py-5 text-center">
											<div className={`
												inline-flex items-center justify-center gap-2 text-[9px] font-black uppercase tracking-widest
												${u.isLocked ? "text-[var(--danger)]" : "text-[var(--success)]"}
											`}>
												{u.isLocked ? <Lock className="w-3 h-3" /> : <Activity className="w-3 h-3 animate-pulse" />}
												{u.isLocked ? t("users.status.locked") : t("users.status.active")}
											</div>
										</td>
										<td className="px-8 py-5 text-right">
											<div className="flex items-center justify-end gap-2 opacity-0 group-hover/row:opacity-100 transition-opacity">
												<button
													onClick={() => handleOpenEdit(u)}
													className="p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--warning)]/10 border border-[var(--warning)]/20 text-[var(--warning)] hover:bg-[var(--warning)] hover:tn-text-primary transition-all shadow-md group border-transparent"
													title="Sửa Thông Tin Thiết Lập"
												>
													<Edit2 className="w-4 h-4" />
												</button>
											</div>
										</td>
									</tr>
								))
							)}
						</tbody>
					</table>
				</div>

				{/* Pagination */}
				<StepPagination
					summary={t("users.pagination.summary", { page, total: totalCount })}
					currentLabel={String(page)}
					canPrev={page > 1}
					canNext={page * 10 < totalCount}
					onPrev={() => setPage((currentPage) => Math.max(1, currentPage - 1))}
					onNext={() => setPage((currentPage) => currentPage + 1)}
				/>
			</GlassCard>
		</div>
	);
}
