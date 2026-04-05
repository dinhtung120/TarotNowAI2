'use client';

import React, { useEffect, useRef } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '../../application/call/useCallStore';
import { useCallContext } from './CallProvider';

export const IncomingCallOverlay = () => {
  const { uiState, session, isCaller, localStream, setEnded: setEndedLocalState } = useCallStore();
  const { respondCall, endCall } = useCallContext();
  const t = useTranslations('Chat.call');

  const localVideoRef = useRef<HTMLVideoElement>(null);

  // Dialog chỉ mở nếu state là incoming hoặc ringing
  const isOpen = uiState === 'incoming' || uiState === 'ringing';

  // FIX LỖI #4: Cập nhật luồng video của chính người dùng lúc đang chờ bên kia bắt máy
  useEffect(() => {
    if (localVideoRef.current && localStream) {
      if (localVideoRef.current.srcObject !== localStream) {
        localVideoRef.current.srcObject = localStream;
      }
    }
  }, [localStream, isOpen]);

  const handleAccept = () => {
    if (session?.id) {
      respondCall(session.id, true);
    }
  };

  const handleDecline = () => {
    if (session?.id) {
      respondCall(session.id, false);
    }
  };

  const handleCancel = () => {
    if (session?.id) {
      endCall(session.id, 'cancelled');
    } else {
      setEndedLocalState();
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm transition-opacity duration-300">
      
      {/* Khung ảnh của mình (Người gọi) lúc chờ bắt máy */}
      {/* FIX: Đặt absolute top-20 right-2.5 để cách mép phải 10px và cách Navbar 80px lề */}
      {isCaller && localStream && (
         <div className="absolute top-20 right-2.5 w-32 h-44 bg-gray-800 rounded-xl overflow-hidden shadow-[0_0_30px_rgba(0,0,0,0.8)] border border-white/20 z-[60] animate-in slide-in-from-top-5">
            <video 
              ref={localVideoRef} 
              autoPlay 
              playsInline 
              muted 
              className="w-full h-full object-cover shadow-inner"
            />
         </div>
      )}

      <div className="bg-gray-900 rounded-2xl p-8 max-w-sm w-full mx-4 shadow-2xl border border-gray-800 flex flex-col items-center animate-in fade-in zoom-in duration-300">
        
        {/* Avatar hoặc hiệu ứng Sóng âm / Vòng chuông */}
        <div className="w-24 h-24 rounded-full bg-gradient-to-tr from-purple-500 to-indigo-500 mb-6 flex animate-pulse items-center justify-center shadow-[0_0_30px_rgba(99,102,241,0.6)]">
          <span className="text-white text-3xl">🔮</span>
        </div>

        <h3 className="text-2xl font-semibold text-white mb-2">
          {isCaller ? t('calling') : t('incoming')}
        </h3>
        <p className="text-gray-400 text-sm mb-8">
          {session?.type === 'video' ? t('video') : t('audio')}
        </p>

        <div className="flex gap-6 w-full justify-center">
          {!isCaller ? (
            <>
              <button 
                onClick={handleDecline}
                className="w-14 h-14 rounded-full bg-red-500 flex items-center justify-center hover:bg-red-600 transition-colors"
                title="Từ chối"
              >
                <svg className="w-6 h-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
              <button 
                onClick={handleAccept}
                className="w-14 h-14 rounded-full bg-green-500 flex items-center justify-center hover:bg-green-600 transition-colors shadow-[0_0_20px_rgba(34,197,94,0.4)]"
                title="Đồng ý"
              >
                <svg className="w-6 h-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                </svg>
              </button>
            </>
          ) : (
            <button 
                onClick={handleCancel}
                className="w-14 h-14 rounded-full bg-red-500 flex items-center justify-center hover:bg-red-600 transition-colors"
                title="Hủy cuộc gọi"
              >
                <svg className="w-6 h-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
            </button>
          )}
        </div>
        
      </div>
    </div>
  );
};
