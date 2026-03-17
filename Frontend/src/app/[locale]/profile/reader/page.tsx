'use client';

import React, { useEffect, useState } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from '@/i18n/routing';
import { getReaderProfile, updateReaderProfile, updateReaderStatus, type ReaderProfile } from '@/actions/readerActions';
import {
  Sparkles, Save, Loader2, Activity, BookOpen, Gem, Star
} from 'lucide-react';
import toast from 'react-hot-toast';
import UserLayout from '@/components/layout/UserLayout';
import { SectionHeader, GlassCard, Button } from '@/components/ui';

export default function ReaderSettingsPage() {
  const router = useRouter();
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
        toast.error('Không tìm thấy thông tin Reader. Bạn đã được duyệt chưa?', {
            style: { background: '#18181b', color: '#fff', border: '1px solid #27272a' }
        });
        router.push('/profile');
      }
      setLoading(false);
    };

    fetchProfile();
  }, [isAuthenticated, user, router]);

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
      toast.success('Lưu thông tin thành công!', {
         style: { background: 'var(--success-bg)', color: 'var(--success)', border: '1px solid var(--success)' }
      });
    } else {
      toast.error('Lưu thông tin thất bại!', {
         style: { background: 'var(--danger-bg)', color: 'var(--danger)', border: '1px solid var(--danger)' }
      });
    }
    setSaving(false);
  };

  const handleStatusChange = async (newStatus: string) => {
    setStatus(newStatus);
    const ok = await updateReaderStatus(newStatus);
    if (ok) {
      toast.success('Đã cập nhật trạng thái', {
         style: { background: 'var(--success-bg)', color: 'var(--success)', border: '1px solid var(--success)' }
      });
    } else {
      toast.error('Lỗi cập nhật trạng thái', {
         style: { background: 'var(--danger-bg)', color: 'var(--danger)', border: '1px solid var(--danger)' }
      });
    }
  };

  if (loading) {
    return (
      <UserLayout>
        <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
          <div className="relative group">
            <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--warning)]/20 blur-[60px] rounded-full animate-pulse" />
            <Loader2 className="w-12 h-12 animate-spin text-[var(--warning)] relative z-10" />
          </div>
          <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">Đang tải cấu hình Reader...</div>
        </div>
      </UserLayout>
    );
  }

  return (
    <UserLayout>
      <div className="max-w-3xl mx-auto px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
        
        {/* Header */}
        <SectionHeader
          tag="Reader Settings"
          tagIcon={<Sparkles className="w-3 h-3 text-[var(--warning)]" />}
          title="Giao Diện Reader"
          subtitle="Quản lý thông tin hiển thị và trạng thái hoạt động của bạn."
        />

        <div className="space-y-8">
          {/* Trạng thái hoạt động */}
          <GlassCard className="!p-8">
            <h3 className="text-lg font-black text-white italic tracking-tight mb-6 flex items-center gap-2.5">
              <Activity className="w-5 h-5 text-[var(--warning)]" />
              Trạng Thái Trực Tuyến
            </h3>
            
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <button
                onClick={() => handleStatusChange('accepting_questions')}
                className={`p-5 rounded-2xl border text-center transition-all shadow-lg ${
                  status === 'accepting_questions' 
                    ? 'bg-[var(--success)]/10 border-[var(--success)]/40 ring-1 ring-[var(--success)]/20' 
                    : 'bg-white/[0.02] border-white/5 hover:bg-white/[0.04] hover:border-[var(--success)]/20'
                }`}
              >
                <div className={`w-3.5 h-3.5 mx-auto rounded-full mb-3 shadow-[0_0_10px_currentColor] ${status === 'accepting_questions' ? 'bg-[var(--success)] text-[var(--success)] animate-pulse' : 'bg-zinc-700 text-zinc-700'}`} />
                <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'accepting_questions' ? 'text-[var(--success)]' : 'text-[var(--text-secondary)]'}`}>Đang nhận khách</div>
              </button>
              
              <button
                onClick={() => handleStatusChange('online')}
                className={`p-5 rounded-2xl border text-center transition-all shadow-lg ${
                  status === 'online' 
                    ? 'bg-[var(--warning)]/10 border-[var(--warning)]/40 ring-1 ring-[var(--warning)]/20' 
                    : 'bg-white/[0.02] border-white/5 hover:bg-white/[0.04] hover:border-[var(--warning)]/20'
                }`}
              >
                <div className={`w-3.5 h-3.5 mx-auto rounded-full mb-3 shadow-[0_0_10px_currentColor] ${status === 'online' ? 'bg-[var(--warning)] text-[var(--warning)]' : 'bg-zinc-700 text-zinc-700'}`} />
                <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'online' ? 'text-[var(--warning)]' : 'text-[var(--text-secondary)]'}`}>Online (Bận)</div>
              </button>
              
              <button
                onClick={() => handleStatusChange('offline')}
                className={`p-5 rounded-2xl border text-center transition-all shadow-lg ${
                  status === 'offline' 
                    ? 'bg-[var(--text-secondary)]/10 border-[var(--text-secondary)]/30 ring-1 ring-[var(--text-secondary)]/10' 
                    : 'bg-white/[0.02] border-white/5 hover:bg-white/[0.04] hover:border-[var(--text-secondary)]/20'
                }`}
              >
                <div className={`w-3.5 h-3.5 mx-auto rounded-full mb-3 shadow-[0_0_10px_currentColor] ${status === 'offline' ? 'bg-[var(--text-secondary)] text-[var(--text-secondary)]' : 'bg-zinc-800 text-zinc-800'}`} />
                <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'offline' ? 'text-white' : 'text-[var(--text-secondary)]'}`}>Ngoại tuyến</div>
              </button>
            </div>
          </GlassCard>

          {/* Form cập nhật Info */}
          <GlassCard className="!p-8">
            <form onSubmit={handleSave} className="space-y-8">
              <h3 className="text-lg font-black text-white italic tracking-tight mb-6 flex items-center gap-2.5">
                <BookOpen className="w-5 h-5 text-[var(--purple-accent)]" />
                Hồ Sơ Công Khai
              </h3>

              <div className="space-y-6">
                {/* Bio */}
                <div className="space-y-2">
                  <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">Giới thiệu bản thân (Bio)</label>
                  <textarea
                    value={bioVi}
                    onChange={e => setBioVi(e.target.value)}
                    rows={4}
                    placeholder="Hãy giới thiệu phong cách đọc bài, kinh nghiệm của bạn..."
                    className="w-full bg-white/[0.02] border border-white/10 rounded-xl px-4 py-3.5 text-sm text-white focus:outline-none focus:border-[var(--purple-accent)]/50 focus:ring-1 focus:ring-[var(--purple-accent)]/20 transition-all shadow-inner resize-none"
                  />
                </div>

                {/* Specialties */}
                <div className="space-y-2">
                  <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">Chuyên môn (Cách nhau bởi dấu phẩy)</label>
                  <input
                    type="text"
                    value={specialtiesStr}
                    onChange={e => setSpecialtiesStr(e.target.value)}
                    placeholder="Tình yêu, Sự nghiệp, Tài chính..."
                    className="w-full bg-white/[0.02] border border-white/10 rounded-xl px-4 py-3.5 text-sm text-white focus:outline-none focus:border-[var(--purple-accent)]/50 focus:ring-1 focus:ring-[var(--purple-accent)]/20 transition-all shadow-inner"
                  />
                </div>

                {/* Price */}
                <div className="space-y-2">
                  <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1 flex justify-between items-center">
                    <span>Giá Diamond / 1 Câu hỏi phụ</span>
                    <span className="text-[var(--warning)] font-bold flex items-center gap-1"><Gem className="w-3 h-3"/> {diamondPerQuestion} </span>
                  </label>
                  <div className="relative">
                    <Gem className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--warning)]" />
                    <input
                      type="number"
                      value={diamondPerQuestion}
                      onChange={e => setDiamondPerQuestion(Number(e.target.value))}
                      min={50}
                      className="w-full pl-12 pr-4 py-3.5 bg-white/[0.02] border border-white/10 rounded-xl text-sm text-white focus:outline-none focus:border-[var(--warning)]/50 focus:ring-1 focus:ring-[var(--warning)]/20 transition-all font-bold shadow-inner"
                    />
                  </div>
                  <p className="text-[10px] text-[var(--text-tertiary)] italic pl-1">Câu hỏi chính của Session luôn được tính phí cố định qua Escrow.</p>
                </div>
              </div>

              {/* Submit */}
              <div className="pt-6 border-t border-white/10 mt-6">
                <Button
                  variant="primary"
                  type="submit"
                  disabled={saving}
                  className="w-full h-12"
                >
                  {saving ? (
                    <>
                      <Loader2 className="w-4 h-4 animate-spin mr-2" />
                      Lưu thay đổi...
                    </>
                  ) : (
                    <>
                      <Save className="w-4 h-4 mr-2" />
                      Lưu Hồ Sơ
                    </>
                  )}
                </Button>
              </div>
            </form>
          </GlassCard>
        </div>
      </div>
    </UserLayout>
  );
}
