'use client';

import React from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { getReaderProfile, updateReaderStatus } from '@/features/reader/public';
import { normalizeReaderStatus, type ReaderStatus } from '@/features/reader/domain/readerStatus';
import { Loader2 } from 'lucide-react';
import toast from 'react-hot-toast';

/*
 * Component đóng vai trò như một nút chuyển (Switch button) trạng thái Bận/Sẵn sàng.
 * - Giao tiếp với API để lấy và cập nhật trạng thái Reader.
 * - Hiển thị màu sắc khác nhau tùy theo trạng thái `busy` hay `online`.
 */
export default function ReaderBusyToggle() {
 const user = useAuthStore(state => state.user);
 const queryClient = useQueryClient();

 // Gọi API lấy thông tin profile để biết status hiện hành của reader
 const profileQuery = useQuery({
  queryKey: ['reader-profile-settings', user?.id],
  enabled: !!user,
  queryFn: async () => {
   if (!user) return null;
   const result = await getReaderProfile(user.id);
   return result.success ? result.data ?? null : null;
  },
 });

 // Khởi tạo mutation để thay đổi trạng thái
 const statusMutation = useMutation({
  mutationFn: updateReaderStatus,
 });

 const currentStatus = normalizeReaderStatus(profileQuery.data?.status);
 const isBusy = currentStatus === 'busy';
 const isLoading = profileQuery.isLoading || statusMutation.isPending;

 // Hàm xử lý khi nhấn nút thay đổi trạng thái
 const handleToggle = async () => {
  // Backend không cho phép tự đẩy 'online', phải dùng 'offline' để hủy 'busy'. 
  // Sau đó PresenceHub sẽ tự động nhận diện online.
  const newStatus: ReaderStatus = isBusy ? 'offline' : 'busy';
  const result = await statusMutation.mutateAsync(newStatus);
  
  if (result.success) {
   // Cập nhật lại cache để dữ liệu đồng bộ
   await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
  } else {
   toast.error('Cập nhật trạng thái thất bại', {
    style: {
     background: 'var(--danger-bg)',
     color: 'var(--danger)',
     border: '1px solid var(--danger)',
    },
   });
  }
 };

 return (
  <div
   className="inline-flex w-fit items-center gap-3 transition-all ml-auto"
   title={isBusy ? 'Chuyển sang trạng thái Online (Sẵn sàng)' : 'Chuyển sang trạng thái Bận'}
  >
   <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
    Trạng thái: <span className={`ml-1 ${isBusy ? 'text-[var(--warning)]' : 'text-[var(--success)]'}`}>{isBusy ? 'Bận' : 'Sẵn sàng'}</span>
   </span>
   
   {/* Nút gạt trái phải (Switch button) */}
   <button
    role="switch"
    aria-checked={isBusy}
    onClick={handleToggle}
    disabled={isLoading}
    className={`relative inline-flex h-6 w-11 shrink-0 cursor-pointer items-center rounded-full border-2 border-transparent transition-colors duration-300 ease-in-out focus:outline-none focus-visible:ring-2 focus-visible:ring-offset-2 focus-visible:ring-opacity-75 ${
     isBusy ? 'bg-[var(--warning)] focus-visible:ring-[var(--warning)]' : 'bg-[var(--success)] focus-visible:ring-[var(--success)]'
    } ${isLoading ? 'opacity-50 cursor-not-allowed' : 'hover:brightness-110 active:scale-95'}`}
   >
    <span className="sr-only">Toggle Busy Status</span>
    <span
     className={`pointer-events-none flex h-5 w-5 transform items-center justify-center rounded-full bg-white shadow-lg ring-0 transition duration-300 ease-in-out ${
      isBusy ? 'translate-x-5' : 'translate-x-0'
     }`}
    >
     {isLoading && <Loader2 className="w-3 h-3 text-slate-500 animate-spin" />}
    </span>
   </button>
  </div>
 );
}
