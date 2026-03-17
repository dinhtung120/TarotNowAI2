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
 * 1. Nebula Blobs — gradient lunar + botanical tạo hiệu ứng chữa lành
 * 2. Noise Overlay — texture hạt mịn cho chiều sâu
 * 3. Grid Overlay — lưới mờ nhẹ (chỉ ở variant intense)
 * 4. Spiritual Particles — hạt sáng bay lên với vị trí deterministic
 *
 * Tại sao dùng "use client"?
 * → Background dùng animation CSS và token theme global.
 * → Particles dùng công thức deterministic để luôn đồng bộ SSR/CSR.
 */

/**
 * Noise deterministic để thay Math.random trong render.
 * Giữ UI ổn định qua mọi lần render và không vi phạm purity rules.
 */
const stableNoise = (seed: number) => {
 const x = Math.sin(seed * 12.9898) * 43758.5453;
 return x - Math.floor(x);
};

const getParticleStyle = (index: number) => ({
 top: `${stableNoise(index + 1) * 100}%`,
 left: `${stableNoise(index + 101) * 100}%`,
 animationDuration: `${20 + stableNoise(index + 201) * 35}s`,
 animationDelay: `${-stableNoise(index + 301) * 20}s`,
});

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
 /* Tính số particles dựa trên variant nếu không truyền prop */
 const resolvedParticleCount =
 particleCount ??
 (variant === "subtle" ? 8 : variant === "intense" ? 25 : 15);

  /**
   * Toàn bộ màu nebula được lấy từ global token để đổi theme tại 1 nơi.
   */
  const nebulaTone =
    variant === "subtle"
      ? {
          purple: "var(--nebula-purple-subtle)",
          mint: "var(--nebula-mint-subtle)",
          moon: "var(--nebula-moon-subtle)",
        }
      : variant === "intense"
        ? {
            purple: "var(--nebula-purple-intense)",
            mint: "var(--nebula-mint-intense)",
            moon: "var(--nebula-moon-intense)",
          }
        : {
            purple: "var(--nebula-purple-default)",
            mint: "var(--nebula-mint-default)",
            moon: "var(--nebula-moon-default)",
          };

 return (
 <div
 className="fixed inset-0 z-0 pointer-events-none"
 aria-hidden="true" /* Ẩn khỏi screen reader — purely decorative */
 >
 {/* --- BACKGROUND OVERLAY ---
 Không dùng external noise + mix-blend để tránh lỗi phủ trắng trên Chrome. */}
 <div className="absolute inset-0 opacity-100 bg-[radial-gradient(120%_90%_at_10%_0%,var(--c-215-189-226-50)_0%,transparent_54%),radial-gradient(90%_75%_at_85%_8%,var(--c-178-232-214-42)_0%,transparent_56%),radial-gradient(100%_80%_at_50%_102%,var(--c-255-250-205-36)_0%,transparent_60%)]" />

 {/* --- NEBULA GLOW BLOBS ---
 3 blob gradient lớn, blur mạnh, tạo hiệu ứng thiên hà.
 Mỗi blob có:
 - Vị trí khác nhau (top-left, top-right, bottom)
 - Màu khác nhau (purple, indigo, fuchsia)
 - Animation drift/pulse khác nhau → chuyển động không đều = tự nhiên */}
      <div
        className="absolute top-[-20%] -left-1/4 w-[80vw] h-[80vw] blur-[150px] rounded-full animate-drift"
        style={{ backgroundColor: nebulaTone.purple }}
      />
      <div
        className="absolute top-1/4 -right-1/4 w-[70vw] h-[70vw] blur-[140px] rounded-full animate-drift-reverse"
        style={{ backgroundColor: nebulaTone.mint }}
      />

 {/* Blob thứ 3 chỉ hiện ở variant default và intense */}
 {variant !== "subtle" && (
        <div
          className="absolute -bottom-1/4 left-1/3 w-[60vw] h-[60vw] blur-[130px] rounded-full animate-slow-pulse"
          style={{ backgroundColor: nebulaTone.moon }}
        />
      )}

 {/* --- GRID OVERLAY ---
 Lưới mờ nhẹ — chỉ dùng ở variant intense (Home page).
 Tạo cảm giác "digital" / "holographic" kết hợp với Astral theme. */}
 {variant === "intense" && (
 <div className="absolute inset-0 opacity-[0.08] bg-[linear-gradient(to_right,var(--c-hex-a89cff28)_1px,transparent_1px),linear-gradient(to_bottom,var(--c-hex-b2e8d628)_1px,transparent_1px)] bg-[size:64px_64px]" />
 )}

 {/* --- SPIRITUAL PARTICLES ---
 Hạt sáng nhỏ bay lên dần dần, tạo hiệu ứng "magical".
 Vị trí/tốc độ được tính deterministic theo index để tránh mismatch. */}
 <div className="absolute inset-0">
 {Array.from({ length: resolvedParticleCount }).map((_, i) => (
 <div
 key={i}
 className="absolute w-[2px] h-[2px] bg-[var(--holo-silver)] rounded-full animate-float opacity-[0.4] shadow-[0_0_10px_var(--c-168-156-255-55)]"
 style={getParticleStyle(i)}
 />
 ))}
 </div>
 </div>
 );
}
