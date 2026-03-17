'use client';

import React, { useEffect, useState } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from '@/i18n/routing';
import { getReaderProfile, updateReaderProfile, updateReaderStatus, type ReaderProfile } from '@/actions/readerActions';
import {
  Sparkles, Save, Loader2, Link as LinkIcon, AlertTriangle, BookOpen, Star, Gem, Activity
} from 'lucide-react';
import toast from 'react-hot-toast';

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
         style: { background: '#18181b', color: '#34d399', border: '1px solid #064e3b' }
      });
    } else {
      toast.error('Lưu thông tin thất bại!', {
         style: { background: '#18181b', color: '#f87171', border: '1px solid #7f1d1d' }
      });
    }
    setSaving(false);
  };

  const handleStatusChange = async (newStatus: string) => {
    setStatus(newStatus);
    const ok = await updateReaderStatus(newStatus);
    if (ok) {
      toast.success('Đã cập nhật trạng thái', {
         style: { background: '#18181b', color: '#34d399', border: '1px solid #064e3b' }
      });
    } else {
      toast.error('Lỗi cập nhật trạng thái', {
         style: { background: '#18181b', color: '#f87171', border: '1px solid #7f1d1d' }
      });
    }
  };

  if (loading) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center space-y-4">
        <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
        <span className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">Đang tải cấu hình...</span>
      </div>
    );
  }

  return (
    <div className="max-w-3xl mx-auto px-6 py-16 space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="space-y-4">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md">
          <Sparkles className="w-3 h-3" />
          Reader settings
        </div>
        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
          Giao Diện Reader
        </h1>
        <p className="text-zinc-500 font-medium text-sm">
          Quản lý thông tin hiển thị và trạng thái hoạt động của bạn.
        </p>
      </div>

      {/* Trạng thái hoạt động */}
      <div className="p-8 border border-white/10 bg-white/[0.02] rounded-3xl space-y-6">
        <div className="flex items-center gap-2 text-white font-black italic text-lg uppercase">
          <Activity className="w-5 h-5 text-amber-500" />
          Trạng Thái Trực Tuyến
        </div>
        
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <button
            onClick={() => handleStatusChange('accepting_questions')}
            className={`p-4 rounded-2xl border text-center transition-all ${
              status === 'accepting_questions' 
                ? 'bg-emerald-500/10 border-emerald-500/30' 
                : 'bg-black/40 border-white/5 hover:border-emerald-500/20'
            }`}
          >
            <div className={`w-3 h-3 mx-auto rounded-full mb-2 ${status === 'accepting_questions' ? 'bg-emerald-400 animate-pulse' : 'bg-emerald-900'}`} />
            <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'accepting_questions' ? 'text-emerald-400' : 'text-zinc-500'}`}>Đang nhận khách</div>
          </button>
          
          <button
            onClick={() => handleStatusChange('online')}
            className={`p-4 rounded-2xl border text-center transition-all ${
              status === 'online' 
                ? 'bg-amber-500/10 border-amber-500/30' 
                : 'bg-black/40 border-white/5 hover:border-amber-500/20'
            }`}
          >
            <div className={`w-3 h-3 mx-auto rounded-full mb-2 ${status === 'online' ? 'bg-amber-400' : 'bg-amber-900'}`} />
            <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'online' ? 'text-amber-400' : 'text-zinc-500'}`}>Online (Bận)</div>
          </button>
          
          <button
            onClick={() => handleStatusChange('offline')}
            className={`p-4 rounded-2xl border text-center transition-all ${
              status === 'offline' 
                ? 'bg-zinc-500/10 border-zinc-500/30' 
                : 'bg-black/40 border-white/5 hover:border-zinc-500/20'
            }`}
          >
            <div className={`w-3 h-3 mx-auto rounded-full mb-2 ${status === 'offline' ? 'bg-zinc-400' : 'bg-zinc-800'}`} />
            <div className={`text-[10px] font-black uppercase tracking-widest ${status === 'offline' ? 'text-zinc-400' : 'text-zinc-500'}`}>Ngoại tuyến</div>
          </button>
        </div>
      </div>

      {/* Form cập nhật Info */}
      <form onSubmit={handleSave} className="p-8 border border-white/10 bg-white/[0.02] rounded-3xl space-y-8">
        <div className="flex items-center gap-2 text-white font-black italic text-lg uppercase mb-6">
          <BookOpen className="w-5 h-5 text-purple-400" />
          Hồ Sơ Công Khai
        </div>

        <div className="space-y-6">
          {/* Bio */}
          <div className="space-y-2">
             <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Giới thiệu bản thân (Bio)</label>
             <textarea
               value={bioVi}
               onChange={e => setBioVi(e.target.value)}
               rows={4}
               placeholder="Hãy giới thiệu phong cách đọc bài, kinh nghiệm của bạn..."
               className="w-full bg-black/40 border border-white/10 rounded-xl px-4 py-3 text-sm text-white focus:outline-none focus:border-purple-500/50 transition-all resize-none"
             />
          </div>

          {/* Specialties */}
          <div className="space-y-2">
             <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Chuyên môn (Cách nhau bởi dấu phẩy)</label>
             <input
               type="text"
               value={specialtiesStr}
               onChange={e => setSpecialtiesStr(e.target.value)}
               placeholder="Tình yêu, Sự nghiệp, Tài chính..."
               className="w-full bg-black/40 border border-white/10 rounded-xl px-4 py-3 text-sm text-white focus:outline-none focus:border-purple-500/50 transition-all"
             />
          </div>

          {/* Price */}
          <div className="space-y-2">
             <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Giá Diamond / 1 Câu hỏi phụ</label>
             <div className="relative">
               <Gem className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-amber-400" />
               <input
                 type="number"
                 value={diamondPerQuestion}
                 onChange={e => setDiamondPerQuestion(Number(e.target.value))}
                 min={50}
                 className="w-full pl-12 pr-4 py-3 bg-black/40 border border-white/10 rounded-xl text-sm text-white focus:outline-none focus:border-amber-500/50 transition-all font-bold"
               />
             </div>
             <p className="text-xs text-zinc-600 italic">Câu hỏi chính của Session luôn được tính phí cố định qua Escrow.</p>
          </div>
        </div>

        {/* Submit */}
        <button
          type="submit"
          disabled={saving}
          className="w-full group overflow-hidden bg-gradient-to-r from-purple-600 to-purple-500 hover:from-purple-500 hover:to-purple-400 rounded-2xl p-4 text-center font-black uppercase tracking-widest text-sm text-white transition-all shadow-xl disabled:opacity-50"
        >
          {saving ? (
             <span className="flex items-center justify-center gap-2">
               <Loader2 className="w-4 h-4 animate-spin" /> Lưu thay đổi...
             </span>
          ) : (
             <span className="flex items-center justify-center gap-2">
               <Save className="w-4 h-4" /> Lưu Hồ Sơ
             </span>
          )}
        </button>
      </form>
    </div>
  );
}
