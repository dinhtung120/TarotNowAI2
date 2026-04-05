'use client';

import { usePathname } from 'next/navigation';
import ConversationSidebar from '@/features/chat/presentation/components/ConversationSidebar';
import { CallProvider, IncomingCallOverlay, ActiveCallOverlay } from '@/features/chat/presentation/call';

/*
 * File: chat/layout.tsx
 * BỐI CẢNH (CONTEXT):
 *   Layout chuyên biệt cho tính năng Chat. Cung cấp cấu trúc Master-Detail.
 * 
 * THIẾT KẾ:
 *   - Desktop: Hiển thị 2 cột cố định. Cột trái là danh sách (Sidebar), cột phải là nội dung chi tiết.
 *   - Mobile: 
 *     + Tại route /chat: Chỉ hiện danh sách (Sidebar).
 *     + Tại route /chat/[id]: Chỉ hiện nội dung chi tiết (Children).
 */

export default function ChatLayout({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  
  // Kiểm tra xem người dùng có đang ở trang chi tiết cuộc trò chuyện hay không (/chat/[id])
  // Chúng ta dựa vào việc pathname có chứa ID phía sau /chat/ hay không.
  const isChatRoom = pathname.split('/').filter(Boolean).length > 2;

  return (
    <CallProvider>
      <div className="flex h-full w-full overflow-hidden bg-[#050505]/40 backdrop-blur-sm">
        {/* 
          Sidebar: 
          - Luôn hiện trên desktop (md:flex). 
          - Trên mobile: Chỉ hiện nếu KHÔNG phải đang ở trong phòng chat (!isChatRoom).
        */}
        <aside className={`
          ${isChatRoom ? 'hidden md:flex' : 'flex'} 
          w-full md:w-[350px] lg:w-[400px] border-r border-white/5 flex-col shrink-0
        `}>
          <ConversationSidebar />
        </aside>

        {/* 
          Main Detail Area (Children):
          - Luôn hiện trên desktop (md:flex).
          - Trên mobile: Chỉ hiện nếu ĐANG ở trong phòng chat (isChatRoom).
        */}
        <main className={`
          ${isChatRoom ? 'flex' : 'hidden md:flex'} 
          flex-1 min-w-0 flex-col relative
        `}>
          {children}
        </main>
      </div>

      <IncomingCallOverlay />
      <ActiveCallOverlay />
    </CallProvider>
  );
}
