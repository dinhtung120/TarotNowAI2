"use client";

import { useCallback, useMemo, useState } from "react";
import {
 Ticket,
 X,
 Trash2,
 CheckCircle2,
 Power,
 AlertTriangle,
 Coins,
 Sparkles,
 Loader2,
 PlusCircle,
} from "lucide-react";
import { SectionHeader, GlassCard, Button } from "@/components/ui";
import toast from "react-hot-toast";
import { useLocale, useTranslations } from "next-intl";
import {
 listPromotions as listPromotionsAction,
 createPromotion as createPromotionAction,
 updatePromotion as updatePromotionAction,
 deletePromotion as deletePromotionAction,
 type DepositPromotion,
} from "@/actions/promotionActions";

interface AdminPromotionsClientProps {
 initialPromotions: DepositPromotion[];
}

type Promotion = DepositPromotion;

export default function AdminPromotionsClient({ initialPromotions }: AdminPromotionsClientProps) {
 const t = useTranslations("Admin");
 const locale = useLocale();
 const [promotions, setPromotions] = useState<Promotion[]>(initialPromotions);
 const [loading, setLoading] = useState(false);

 // Form states
 const [isCreating, setIsCreating] = useState(false);
 const [minAmount, setMinAmount] = useState<number>(0);
 const [bonusGold, setBonusGold] = useState<number>(0);
 const [deleteId, setDeleteId] = useState<string | null>(null);
 const [submitting, setSubmitting] = useState(false);
 const [togglingId, setTogglingId] = useState<string | null>(null);
 const [deletingId, setDeletingId] = useState<string | null>(null);

 const moneyFormatter = useMemo(
  () => new Intl.NumberFormat(locale, { style: "currency", currency: "VND", maximumFractionDigits: 0 }),
  [locale],
 );

 const formatMoney = useCallback((value: number) => moneyFormatter.format(value), [moneyFormatter]);

 const refreshPromotions = useCallback(async (options?: { showLoading?: boolean }) => {
  const showLoading = options?.showLoading ?? true;
  if (showLoading) setLoading(true);

  try {
   const data = await listPromotionsAction(false);
   setPromotions(data ?? []);
  } catch (err) {
   console.error(err);
   setPromotions([]);
  } finally {
   if (showLoading) setLoading(false);
  }
 }, []);

 const handleCreate = async (e: React.FormEvent) => {
  e.preventDefault();
  setSubmitting(true);
  try {
   const ok = await createPromotionAction(minAmount, bonusGold);
   if (ok) {
    setIsCreating(false);
    setMinAmount(0);
    setBonusGold(0);
    toast.success(t("promotions.toast.create_success"));
    await refreshPromotions({ showLoading: false });
   } else {
    toast.error(t("promotions.toast.create_failed"));
   }
  } catch {
   toast.error(t("promotions.toast.network_error"));
  } finally {
   setSubmitting(false);
  }
 };

 const handleToggle = async (promotion: Promotion) => {
  setTogglingId(promotion.id);
  try {
   const nextActive = !promotion.isActive;
   const ok = await updatePromotionAction(promotion.id, {
    minAmountVnd: promotion.minAmountVnd,
    bonusDiamond: promotion.bonusDiamond,
    isActive: nextActive,
   });
   if (ok) {
    setPromotions((prev) =>
     prev.map((item) => (item.id === promotion.id ? { ...item, isActive: nextActive } : item)),
    );
    toast.success(t("promotions.toast.toggle_success"));
   } else {
    toast.error(t("promotions.toast.toggle_failed"));
   }
  } catch {
   toast.error(t("promotions.toast.network_error"));
  } finally {
   setTogglingId(null);
  }
 };

 const handleDelete = async () => {
  if (!deleteId) return;
  setDeletingId(deleteId);
  try {
   const ok = await deletePromotionAction(deleteId);
   if (ok) {
    setPromotions((prev) => prev.filter((item) => item.id !== deleteId));
    setDeleteId(null);
    toast.success(t("promotions.toast.delete_success"));
   } else {
    toast.error(t("promotions.toast.delete_failed"));
   }
  } catch {
   toast.error(t("promotions.toast.network_error"));
  } finally {
   setDeletingId(null);
  }
 };

 return (
  <div className="space-y-8 pb-20 animate-in fade-in duration-700">
   {/* Header Area */}
   <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
    <SectionHeader
     tag={t("promotions.header.tag")}
     tagIcon={<Ticket className="w-3 h-3 text-[var(--warning)]" />}
     title={t("promotions.header.title")}
     subtitle={t("promotions.header.subtitle")}
     className="mb-0 text-left items-start"
    />

    <Button
     variant={isCreating ? "secondary" : "primary"}
     onClick={() => setIsCreating(!isCreating)}
     className={`shrink-0 ${!isCreating && "bg-[var(--warning)] tn-text-ink hover:bg-[var(--warning)] hover:brightness-110 shadow-[0_0_20px_var(--c-245-158-11-20)]"}`}
    >
     {isCreating ? <X className="w-4 h-4" /> : <PlusCircle className="w-4 h-4" />}
     {isCreating ? t("promotions.actions.toggle_create_cancel") : t("promotions.actions.toggle_create_add")}
    </Button>
   </div>

   {/* Create form - Inline Expansion */}
   {isCreating && (
    <div className="animate-in fade-in slide-in-from-top-4 duration-500">
     <GlassCard className="relative overflow-hidden group hover:border-[var(--warning)]/30 transition-all !p-8">
      <div className="absolute top-0 right-0 p-16 bg-[var(--warning)]/10 blur-[100px] rounded-full" />
      <form onSubmit={handleCreate} className="relative z-10 space-y-8">
       <div className="flex items-center gap-3 border-b tn-border-soft pb-4">
        <Sparkles className="w-5 h-5 text-[var(--warning)]" />
        <h2 className="text-sm font-black tn-text-primary uppercase tracking-widest drop-shadow-sm">
         {t("promotions.create.title")}
        </h2>
       </div>

       <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        <div className="space-y-3 text-left">
         <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
          {t("promotions.create.min_amount_label")}
         </label>
         <div className="relative">
          <input
           type="number"
           min="0"
           required
           value={minAmount}
           onChange={(e) => setMinAmount(Number(e.target.value))}
           placeholder={t("promotions.create.min_amount_placeholder")}
           className="w-full tn-field rounded-2xl pl-12 pr-4 py-4 text-xs font-black tn-text-primary tn-field-warning transition-all placeholder:text-[var(--text-tertiary)] shadow-inner"
          />
          <div className="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none">
           <span className="text-sm font-black text-[var(--text-secondary)]">₫</span>
          </div>
         </div>
        </div>

        <div className="space-y-3 text-left">
         <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
          {t("promotions.create.bonus_label")}
         </label>
         <div className="relative">
          <input
           type="number"
           min="0"
           required
           value={bonusGold}
           onChange={(e) => setBonusGold(Number(e.target.value))}
           placeholder={t("promotions.create.bonus_placeholder")}
           className="w-full tn-field rounded-2xl pl-12 pr-4 py-4 text-xs font-black tn-text-primary tn-field-warning transition-all placeholder:text-[var(--text-tertiary)] shadow-inner"
          />
          <div className="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none">
           <Coins className="w-4 h-4 text-[var(--warning)]" />
          </div>
         </div>
        </div>
       </div>

       <div className="flex flex-col sm:flex-row justify-end items-center gap-4 pt-2 border-t tn-border-soft">
        <p className="text-[9px] font-bold text-[var(--text-tertiary)] uppercase tracking-tighter text-center sm:text-right leading-relaxed flex-1">
         {t("promotions.create.note")}
        </p>
        <Button
         type="submit"
         disabled={submitting}
         className="w-full sm:w-auto px-8 py-4 bg-[var(--warning)] tn-text-ink hover:bg-[var(--warning)] hover:brightness-110 shadow-[0_0_20px_var(--c-245-158-11-20)]"
        >
         {submitting ? (
          <Loader2 className="w-4 h-4 animate-spin mr-2" />
         ) : (
          <CheckCircle2 className="w-4 h-4 mr-2" />
         )}
         {submitting ? t("promotions.create.submitting") : t("promotions.create.submit")}
        </Button>
       </div>
      </form>
     </GlassCard>
    </div>
   )}

   {/* List Section */}
   <GlassCard className="!p-0 !rounded-[2.5rem] overflow-hidden text-left">
    <div className="md:hidden p-4 sm:p-6 space-y-3">
     {loading ? (
      <div className="py-16 text-center">
       <div className="flex flex-col items-center justify-center space-y-4">
        <Loader2 className="w-8 h-8 animate-spin text-[var(--warning)]" />
        <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
         {t("promotions.states.loading")}
        </span>
       </div>
      </div>
     ) : promotions.length === 0 ? (
      <div className="py-16 text-center">
       <div className="flex flex-col items-center justify-center space-y-4">
        <div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
         <Ticket className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
        </div>
        <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
         {t("promotions.states.empty")}
        </span>
       </div>
      </div>
     ) : (
      promotions.map((p) => (
       <div key={p.id} className="rounded-2xl tn-panel-soft border tn-border-soft p-4 space-y-4">
        <div className="flex items-center justify-between gap-3">
         <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
          {t("promotions.table.heading_condition")}
         </div>
         <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm text-right">
          {t("promotions.row.condition_from", { amount: formatMoney(p.minAmountVnd) })}
         </div>
        </div>
        <div className="flex items-center justify-between gap-3">
         <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
          {t("promotions.table.heading_reward")}
         </div>
         <div className="flex items-center gap-2 text-sm font-black text-[var(--warning)] italic drop-shadow-sm">
          <Coins className="w-4 h-4" />+{p.bonusDiamond.toLocaleString(locale)}
         </div>
        </div>
        <div className="flex items-center gap-2">
         <button
          onClick={() => handleToggle(p)}
          disabled={togglingId === p.id}
          className={`
 relative inline-flex items-center justify-center gap-2 px-4 py-2.5 min-h-11 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner flex-1 disabled:opacity-60
 ${p.isActive ? "bg-[var(--warning)]/10 border-[var(--warning)]/30 text-[var(--warning)] shadow-md hover:bg-[var(--warning)]/20" : "tn-panel text-[var(--text-secondary)] hover:tn-surface hover:tn-text-primary"}
 `}
         >
          {togglingId === p.id ? <Loader2 className="w-3 h-3 animate-spin" /> : <Power className="w-3 h-3" />}
          {p.isActive ? t("promotions.status.active") : t("promotions.status.inactive")}
         </button>
         <button
          onClick={() => setDeleteId(p.id)}
          className="p-3 min-h-11 min-w-11 rounded-xl text-[var(--text-secondary)] tn-panel-soft hover:tn-text-primary hover:bg-[var(--danger)] hover:border-transparent transition-all shadow-sm group"
          aria-label={t("promotions.delete_modal.delete")}
         >
          <Trash2 className="w-4 h-4 group-hover:scale-110 transition-transform" />
         </button>
        </div>
       </div>
      ))
     )}
    </div>

    <div className="hidden md:block overflow-x-auto custom-scrollbar">
     <table className="w-full text-left">
      <thead>
       <tr className="border-b tn-border-soft tn-surface">
        <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">
         {t("promotions.table.heading_condition")}
        </th>
        <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">
         {t("promotions.table.heading_reward")}
        </th>
        <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">
         {t("promotions.table.heading_status")}
        </th>
        <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right">
         {t("promotions.table.heading_commands")}
        </th>
       </tr>
      </thead>
      <tbody className="divide-y divide-white/5">
       {loading ? (
        <tr>
         <td colSpan={4} className="px-8 py-20 text-center">
          <div className="flex flex-col items-center justify-center space-y-4">
           <Loader2 className="w-8 h-8 animate-spin text-[var(--warning)]" />
           <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
            {t("promotions.states.loading")}
           </span>
          </div>
         </td>
        </tr>
       ) : promotions.length === 0 ? (
        <tr>
         <td colSpan={4} className="px-8 py-20 text-center">
          <div className="flex flex-col items-center justify-center space-y-4">
           <div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
            <Ticket className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
           </div>
           <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
            {t("promotions.states.empty")}
           </span>
          </div>
         </td>
        </tr>
       ) : (
        promotions.map((p) => (
         <tr key={p.id} className="group/row hover:tn-surface transition-colors">
          <td className="px-8 py-6">
           <div className="flex items-center gap-4">
            <div className="w-10 h-10 rounded-xl tn-panel-overlay flex items-center justify-center shadow-inner group-hover/row:scale-110 transition-transform">
             <span className="text-[12px] font-black text-[var(--text-secondary)]">₫</span>
            </div>
            <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">
             {t("promotions.row.condition_from", { amount: formatMoney(p.minAmountVnd) })}
            </div>
           </div>
          </td>
          <td className="px-8 py-6">
           <div className="flex items-center gap-2 text-sm font-black text-[var(--warning)] italic drop-shadow-sm">
            <Coins className="w-4 h-4" />+{p.bonusDiamond.toLocaleString(locale)}
           </div>
          </td>
          <td className="px-8 py-6 text-center">
           <button
            onClick={() => handleToggle(p)}
            disabled={togglingId === p.id}
            className={`
 relative inline-flex items-center gap-2 px-4 py-2.5 min-h-11 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner disabled:opacity-60
 ${p.isActive ? "bg-[var(--warning)]/10 border-[var(--warning)]/30 text-[var(--warning)] shadow-md hover:bg-[var(--warning)]/20" : "tn-panel text-[var(--text-secondary)] hover:tn-surface hover:tn-text-primary"}
 `}
           >
            {togglingId === p.id ? <Loader2 className="w-3 h-3 animate-spin" /> : <Power className="w-3 h-3" />}
            {p.isActive ? t("promotions.status.active") : t("promotions.status.inactive")}
           </button>
          </td>
          <td className="px-8 py-6 text-right">
           <button
            onClick={() => setDeleteId(p.id)}
            className="p-3 min-h-11 min-w-11 rounded-xl text-[var(--text-secondary)] tn-panel-soft hover:tn-text-primary hover:bg-[var(--danger)] hover:border-transparent transition-all shadow-sm group"
           >
            <Trash2 className="w-4 h-4 group-hover:scale-110 transition-transform" />
           </button>
          </td>
         </tr>
        ))
       )}
      </tbody>
     </table>
    </div>
   </GlassCard>

   {/* Delete Confirmation Modal */}
   {deleteId && (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
     <div className="absolute inset-0 tn-overlay-strong transition-opacity" onClick={() => setDeleteId(null)} />
     <div className="relative tn-panel rounded-[2.5rem] p-10 shadow-2xl max-w-sm w-full transform transition-all animate-in fade-in zoom-in duration-300">
      <div className="text-center space-y-6">
       <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] shadow-inner">
        <AlertTriangle className="h-8 w-8" />
       </div>
       <div className="space-y-4 text-left">
        <h3 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter text-center mb-2">
         {t("promotions.delete_modal.title")}
        </h3>
        <p className="text-[11px] font-bold text-[var(--text-secondary)] uppercase leading-relaxed tracking-wide text-center">
         {t("promotions.delete_modal.desc")}
        </p>
       </div>
      </div>
      <div className="flex gap-4 mt-8">
       <Button variant="secondary" onClick={() => setDeleteId(null)} className="flex-1">
        {t("promotions.delete_modal.keep")}
       </Button>
       <Button
        variant="danger"
        onClick={handleDelete}
        disabled={Boolean(deletingId)}
        className="flex-1 shadow-[0_0_20px_var(--c-244-63-94-30)] hover:shadow-[0_0_30px_var(--c-244-63-94-50)]"
       >
        {deletingId ? <Loader2 className="w-4 h-4 animate-spin" /> : null}
        {t("promotions.delete_modal.delete")}
       </Button>
      </div>
     </div>
    </div>
   )}
  </div>
 );
}
