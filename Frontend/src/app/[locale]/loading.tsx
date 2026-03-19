/*
 * ===================================================================
 * FILE: loading.tsx (Global Suspense Fallback)
 * BỐI CẢNH (CONTEXT):
 *   Loading UI mặc định của React Suspense được Next.js tự động bọc xung quanh các Page.
 *   Sử dụng spinner để thông báo cho user khi app đang chuẩn bị (fetch) dữ liệu tĩnh hoặc server side.
 * ===================================================================
 */
import { Loader2 } from "lucide-react";

export default function LocaleLoading() {
  return (
    <div className="flex min-h-[70vh] items-center justify-center">
      <div className="flex flex-col items-center gap-4">
        <Loader2 className="h-10 w-10 animate-spin text-[var(--purple-accent)]" />
        <p className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">
          Loading
        </p>
      </div>
    </div>
  );
}
