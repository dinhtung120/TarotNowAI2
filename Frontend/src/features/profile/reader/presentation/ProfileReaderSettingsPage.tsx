/*
 * ===================================================================
 * FILE: (user)/profile/reader/page.tsx (Cài đặt Hồ Sơ Reader)
 * BỐI CẢNH (CONTEXT):
 *   Trang dành riêng cho User có role Reader để cập nhật hồ sơ hành nghề.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Đổi trạng thái nhận câu hỏi (Accepting/Online/Offline) -> Gọi API `updateReaderStatus`.
 *   - Cập nhật Bio (Giới thiệu bản thân), Specialties (Chuyên môn chuyên ngành).
 *   - Chỉnh sửa mức giá thu phí (Diamond Per Question).
 * ===================================================================
 */
'use client';

import React from 'react';
import {
 Sparkles, Save, Loader2, Activity, BookOpen, Gem
} from 'lucide-react';
import { useTranslations } from 'next-intl';
import { SectionHeader, GlassCard, Button } from '@/shared/components/ui';
import { useProfileReaderSettingsPage } from '@/features/profile/reader/application/useProfileReaderSettingsPage';

interface ReaderSettingsPageProps {
 embedded?: boolean;
}

export default function ReaderSettingsPage({ embedded = false }: ReaderSettingsPageProps) {
 const t = useTranslations("Profile");
 const {
 loading,
 saving,
 bioVi,
 setBioVi,
 diamondPerQuestion,
 setDiamondPerQuestion,
 specialtiesStr,
 setSpecialtiesStr,
 status,
 handleSave,
 handleStatusChange,
 } = useProfileReaderSettingsPage(t);

 if (loading) {
 return (
 <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--warning)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--warning)] relative z-10" />
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t("reader.loading")}</div>
 </div>
 );
 }

 const content = (
 <div className="space-y-8">
 {/* Form cập nhật Info */}
 <GlassCard className="!p-8">
 <form onSubmit={handleSave} className="space-y-8">
 <h3 className="text-lg font-black tn-text-primary italic tracking-tight mb-6 flex items-center gap-2.5">
 <BookOpen className="w-5 h-5 text-[var(--purple-accent)]" />
 {t("reader.public_profile_title")}
 </h3>

 <div className="space-y-6">
 {/* Bio */}
 <div className="space-y-2">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">{t("reader.bio_label")}</label>
 <textarea
 value={bioVi}
 onChange={e => setBioVi(e.target.value)}
 rows={4}
 placeholder={t("reader.bio_placeholder")}
 className="w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary tn-field-accent transition-all shadow-inner resize-none"
 />
 </div>

 {/* Specialties */}
 <div className="space-y-2">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">{t("reader.specialties_label")}</label>
 <input
 type="text"
 value={specialtiesStr}
 onChange={e => setSpecialtiesStr(e.target.value)}
 placeholder={t("reader.specialties_placeholder")}
 className="w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary tn-field-accent transition-all shadow-inner"
 />
 </div>

 {/* Price */}
 <div className="space-y-2">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1 flex justify-between items-center">
 <span>{t("reader.price_label")}</span>
 <span className="text-[var(--warning)] font-bold flex items-center gap-1"><Gem className="w-3 h-3"/> {diamondPerQuestion} </span>
 </label>
 <div className="relative">
 <Gem className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--warning)]" />
 <input
 type="number"
 value={diamondPerQuestion}
 onChange={e => setDiamondPerQuestion(Number(e.target.value))}
 min={50}
 className="w-full pl-12 pr-4 py-3.5 tn-field rounded-xl text-sm tn-text-primary tn-field-warning transition-all font-bold shadow-inner"
 />
 </div>
 <p className="text-[10px] text-[var(--text-tertiary)] italic pl-1">{t("reader.price_help")}</p>
 </div>
 </div>

 {/* Submit */}
 <div className="pt-6 border-t tn-border mt-6">
 <Button
 variant="primary"
 type="submit"
 disabled={saving}
 className="w-full h-12"
 >
 {saving ? (
 <>
 <Loader2 className="w-4 h-4 animate-spin mr-2" />
 {t("reader.saving")}
 </>
 ) : (
 <>
 <Save className="w-4 h-4 mr-2" />
 {t("reader.save")}
 </>
 )}
 </Button>
 </div>
 </form>
 </GlassCard>
 </div>
 );

 if (embedded) {
  return content;
 }

 return (
 <div className="max-w-3xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {/* Header */}
 <SectionHeader
 tag={t("reader.tag")}
 tagIcon={<Sparkles className="w-3 h-3 text-[var(--warning)]" />}
 title={t("reader.title")}
 subtitle={t("reader.subtitle")}
 />

 {content}
 </div>
 );
}
