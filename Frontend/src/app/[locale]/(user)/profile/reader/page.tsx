'use client';

import React, { useEffect, useState } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from '@/i18n/routing';
import { getReaderProfile, updateReaderProfile, updateReaderStatus } from '@/actions/readerActions';
import {
 Sparkles, Save, Loader2, Activity, BookOpen, Gem
} from 'lucide-react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { SectionHeader, GlassCard, Button } from '@/components/ui';

export default function ReaderSettingsPage() {
 const router = useRouter();
 const t = useTranslations("Profile");
 const { isAuthenticated, user } = useAuthStore();
 const [loading, setLoading] = useState(true);
 const [saving, setSaving] = useState(false);
 // States cho form
 const [bioVi, setBioVi] = useState('');
 const [diamondPerQuestion, setDiamondPerQuestion] = useState<number>(100);
 const [specialtiesStr, setSpecialtiesStr] = useState('');
 const [status, setStatus] = useState('offline');

 // Lấy dữ liệu
 useEffect(() => {
 if (!isAuthenticated || !user) {
 router.push('/login');
 return;
 }

 const fetchProfile = async () => {
 // User ID để fetch
 const profile = await getReaderProfile(user.id);
 if (profile) {
 setBioVi(profile.bioVi || '');
 setDiamondPerQuestion(profile.diamondPerQuestion || 100);
 setSpecialtiesStr(profile.specialties?.join(', ') || '');
 setStatus(profile.status || 'offline');
 } else {
 toast.error(t("reader.toast_not_found"), {
 style: {
 background: 'var(--bg-elevated)',
 color: 'var(--text-primary)',
 border: '1px solid var(--border-default)'
 }
 });
 router.push('/profile');
 }
 setLoading(false);
 };

 fetchProfile();
 }, [isAuthenticated, user, router, t]);

 const handleSave = async (e: React.FormEvent) => {
 e.preventDefault();
 setSaving(true);
 // Parse specialties
 const specArray = specialtiesStr
 .split(',')
 .map(s => s.trim())
 .filter(s => s.length > 0);

 const success = await updateReaderProfile({
 bioVi,
 diamondPerQuestion,
 specialties: specArray
 });

 if (success) {
 toast.success(t("reader.toast_save_success"), {
 style: { background: 'var(--success-bg)', color: 'var(--success)', border: '1px solid var(--success)' }
 });
 } else {
 toast.error(t("reader.toast_save_fail"), {
 style: { background: 'var(--danger-bg)', color: 'var(--danger)', border: '1px solid var(--danger)' }
 });
 }
 setSaving(false);
 };

 const handleStatusChange = async (newStatus: string) => {
 setStatus(newStatus);
 const ok = await updateReaderStatus(newStatus);
 if (ok) {
 toast.success(t("reader.toast_status_updated"), {
 style: { background: 'var(--success-bg)', color: 'var(--success)', border: '1px solid var(--success)' }
 });
 } else {
 toast.error(t("reader.toast_status_update_fail"), {
 style: { background: 'var(--danger-bg)', color: 'var(--danger)', border: '1px solid var(--danger)' }
 });
 }
 };

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

 return (
 <div className="max-w-3xl mx-auto px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {/* Header */}
 <SectionHeader
 tag={t("reader.tag")}
 tagIcon={<Sparkles className="w-3 h-3 text-[var(--warning)]" />}
 title={t("reader.title")}
 subtitle={t("reader.subtitle")}
 />

 <div className="space-y-8">
 {/* Trạng thái hoạt động */}
 <GlassCard className="!p-8">
 <h3 className="text-lg font-black tn-text-primary italic tracking-tight mb-6 flex items-center gap-2.5">
 <Activity className="w-5 h-5 text-[var(--warning)]" />
 {t("reader.status_title")}
 </h3>
 <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
 <button
 onClick={() => handleStatusChange('accepting_questions')}
 className={`p-5 rounded-2xl border text-center transition-all shadow-lg ${
 status === 'accepting_questions' ? 'bg-[var(--success)]/10 border-[var(--success)]/40 ring-1 ring-[var(--success)]/20' : 'tn-panel-soft hover:tn-surface-strong hover:border-[var(--success)]/20'
 }`}
 >
 <div className={`w-3.5 h-3.5 mx-auto rounded-full mb-3 shadow-[0_0_10px_currentColor] ${status === 'accepting_questions' ? 'bg-[var(--success)] text-[var(--success)] animate-pulse' : 'tn-surface tn-text-muted'}`} />
 <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'accepting_questions' ? 'text-[var(--success)]' : 'text-[var(--text-secondary)]'}`}>{t("reader.status_accepting")}</div>
 </button>
 <button
 onClick={() => handleStatusChange('online')}
 className={`p-5 rounded-2xl border text-center transition-all shadow-lg ${
 status === 'online' ? 'bg-[var(--warning)]/10 border-[var(--warning)]/40 ring-1 ring-[var(--warning)]/20' : 'tn-panel-soft hover:tn-surface-strong hover:border-[var(--warning)]/20'
 }`}
 >
 <div className={`w-3.5 h-3.5 mx-auto rounded-full mb-3 shadow-[0_0_10px_currentColor] ${status === 'online' ? 'bg-[var(--warning)] text-[var(--warning)]' : 'tn-surface tn-text-muted'}`} />
 <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'online' ? 'text-[var(--warning)]' : 'text-[var(--text-secondary)]'}`}>{t("reader.status_online")}</div>
 </button>
 <button
 onClick={() => handleStatusChange('offline')}
 className={`p-5 rounded-2xl border text-center transition-all shadow-lg ${
 status === 'offline' ? 'bg-[var(--text-secondary)]/10 border-[var(--text-secondary)]/30 ring-1 ring-[var(--text-secondary)]/10' : 'tn-panel-soft hover:tn-surface-strong hover:border-[var(--text-secondary)]/20'
 }`}
 >
 <div className={`w-3.5 h-3.5 mx-auto rounded-full mb-3 shadow-[0_0_10px_currentColor] ${status === 'offline' ? 'bg-[var(--text-secondary)] text-[var(--text-secondary)]' : 'tn-surface-strong tn-text-muted'}`} />
 <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'offline' ? 'tn-text-primary' : 'text-[var(--text-secondary)]'}`}>{t("reader.status_offline")}</div>
 </button>
 </div>
 </GlassCard>

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
 </div>
 );
}
