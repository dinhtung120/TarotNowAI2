"use client";

/**
 * AstralBackground — Component nền vũ trụ dùng chung toàn app.
 *
 * === VẤN ĐỀ TRƯỚC ĐÂY ===
 * Mỗi trang (Home, Reading, Wallet, Profile...) đều COPY-PASTE ~40 dòng code:
 * - Nebula blobs (3-4 div blur gradient)
 * - Noise texture overlay
 * - Spiritual Particles (15-20 hạt bay random)
 * - CSS animations (@keyframes float, drift, pulse-slow)
 *
 * Hậu quả:
 * → ~15 files chứa code gần giống nhau
 * → Muốn đổi màu nebula → sửa 15 files
 * → Particles render trên Server → Hydration Mismatch (Math.random()*100%)
 *
 * === GIẢI PHÁP ===
 * Component này tập trung TẤT CẢ hiệu ứng nền vào 1 nơi:
 * 1. Nebula Blobs — gradient tối tạo hiệu ứng huyền bí
 * 2. Noise Overlay — texture hạt mịn cho chiều sâu
 * 3. Grid Overlay — lưới mờ nhẹ (chỉ ở variant intense)
 * 4. Spiritual Particles — hạt sáng bay lên (client-only, tránh hydration error)
 *
 * Tại sao dùng "use client"?
 * → Particles dùng Math.random() → phải render ở client.
 * → useEffect + mounted state đảm bảo SSR trả về HTML trống cho particles,
 *   client render thêm particles sau → không bao giờ Hydration Mismatch.
 */

import { useEffect, useState } from "react";

/**
 * Các variant cho phép điều chỉnh cường độ hiệu ứng:
 * - "subtle": Ít nebula, ít particles → cho trang có nội dung dày (Chat, Admin)
 * - "default": Cân bằng → cho hầu hết trang (Profile, Wallet, Reading)
 * - "intense": Nhiều nebula, grid overlay, particles → cho trang Hero (Home)
 */
interface AstralBackgroundProps {
  variant?: "subtle" | "default" | "intense";

  /**
   * Số lượng hạt particles.
   * Mặc định: subtle=8, default=15, intense=25
   */
  particleCount?: number;
}

export default function AstralBackground({
  variant = "default",
  particleCount,
}: AstralBackgroundProps) {
  /**
   * Tại sao cần state `mounted`?
   * → Math.random() tạo giá trị KHÁC NHAU giữa Server và Client.
   * → Nếu render particles ngay lập tức, React so sánh HTML Server vs Client
   *   → thấy khác → throws Hydration Mismatch Error.
   * → Giải pháp: Server render div rỗng, Client mount xong mới thêm particles.
   */
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  /* Tính số particles dựa trên variant nếu không truyền prop */
  const resolvedParticleCount =
    particleCount ??
    (variant === "subtle" ? 8 : variant === "intense" ? 25 : 15);

  /**
   * Nebula opacity scale — variant intense sáng hơn, subtle mờ hơn.
   * Dùng opacity fraction (0.05 → 0.12) thay vì full opacity
   * để nebula KHÔNG che nội dung, chỉ tạo ambient light.
   */
  const nebulaAlpha =
    variant === "subtle" ? 0.08 : variant === "intense" ? 0.16 : 0.12;

  return (
    <div
      className="fixed inset-0 z-0 pointer-events-none"
      aria-hidden="true" /* Ẩn khỏi screen reader — purely decorative */
    >
      {/* --- BACKGROUND OVERLAY ---
          Không dùng external noise + mix-blend để tránh lỗi phủ trắng trên Chrome. */}
      <div className="absolute inset-0 opacity-100 bg-[radial-gradient(120%_90%_at_15%_0%,rgba(122,86,177,0.12)_0%,transparent_52%),radial-gradient(90%_75%_at_85%_10%,rgba(61,78,140,0.11)_0%,transparent_55%),linear-gradient(180deg,rgba(5,3,13,0)_0%,rgba(5,3,13,0.45)_100%)]" />

      {/* --- NEBULA GLOW BLOBS ---
          3 blob gradient lớn, blur mạnh, tạo hiệu ứng thiên hà.
          Mỗi blob có:
          - Vị trí khác nhau (top-left, top-right, bottom)
          - Màu khác nhau (purple, indigo, fuchsia)
          - Animation drift/pulse khác nhau → chuyển động không đều = tự nhiên */}
      <div
        className="absolute top-[-20%] -left-1/4 w-[80vw] h-[80vw] blur-[150px] rounded-full animate-drift"
        style={{ backgroundColor: `rgba(56, 28, 98, ${nebulaAlpha})` }}
      />
      <div
        className="absolute top-1/4 -right-1/4 w-[70vw] h-[70vw] blur-[140px] rounded-full animate-drift-reverse"
        style={{ backgroundColor: `rgba(37, 44, 102, ${nebulaAlpha})` }}
      />

      {/* Blob thứ 3 chỉ hiện ở variant default và intense */}
      {variant !== "subtle" && (
        <div
          className="absolute -bottom-1/4 left-1/3 w-[60vw] h-[60vw] blur-[130px] rounded-full animate-slow-pulse"
          style={{ backgroundColor: `rgba(70, 31, 108, ${nebulaAlpha})` }}
        />
      )}

      {/* --- GRID OVERLAY ---
          Lưới mờ nhẹ — chỉ dùng ở variant intense (Home page).
          Tạo cảm giác "digital" / "holographic" kết hợp với Astral theme. */}
      {variant === "intense" && (
        <div className="absolute inset-0 opacity-[0.06] bg-[linear-gradient(to_right,#8f5bc320_1px,transparent_1px),linear-gradient(to_bottom,#8f5bc320_1px,transparent_1px)] bg-[size:60px_60px]" />
      )}

      {/* --- SPIRITUAL PARTICLES ---
          Hạt sáng nhỏ bay lên dần dần, tạo hiệu ứng "magical".

          Tại sao chỉ render khi mounted?
          → Math.random() trong style inline tạo giá trị KHÁC giữa SSR và CSR.
          → Server render: top=47.3%, left=82.1%
          → Client render: top=15.9%, left=33.5% (random hoàn toàn khác)
          → React thấy khác → Hydration Error!

          Giải pháp:
          → Server: render null (div rỗng)
          → Client: useEffect set mounted=true → render particles với random values
          → Không bao giờ mismatch vì Server không render particles. */}
      {mounted && (
        <div className="absolute inset-0">
          {Array.from({ length: resolvedParticleCount }).map((_, i) => (
            <div
              key={i}
              className="absolute w-[1px] h-[1px] bg-[var(--purple-accent)] rounded-full animate-float opacity-[0.24]"
              style={{
                top: `${Math.random() * 100}%`,
                left: `${Math.random() * 100}%`,
                /* Duration ngẫu nhiên 20-55s → mỗi hạt bay khác tốc độ */
                animationDuration: `${20 + Math.random() * 35}s`,
                /* Delay âm → hạt bắt đầu ở giữa animation, không đợi từ đầu */
                animationDelay: `${-Math.random() * 20}s`,
              }}
            />
          ))}
        </div>
      )}
    </div>
  );
}
