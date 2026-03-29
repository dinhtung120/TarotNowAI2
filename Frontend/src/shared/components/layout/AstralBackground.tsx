"use client";

/*
 * ===================================================================
 * COMPONENT: AstralBackground
 * BỐI CẢNH (CONTEXT):
 *   Component Nền Vũ Trụ (Astral) dùng chung cho toàn bộ ứng dụng để tạo 
 *   hiệu ứng chiều sâu, linh thiêng và huyền bí đặc trưng của TarotNow.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Gom chung toàn bộ logic render Nebula Blobs (đám mây thiên hà) và 
 *     Spiritual Particles (hạt bụi sáng) vào một nơi, tránh copy-paste code.
 *   - Sử dụng hàm `stableNoise` (thay vì Math.random) để giữ cho vị trí 
 *     các hạt luôn cố định (Deterministic) giữa Server và Client, chống lỗi Hydration.
 *   - Cung cấp 3 cấp độ `variant` (Subtle, Default, Intense) cho từng loại trang.
 * ===================================================================
 */

/**
 * Noise deterministic để thay Math.random trong render.
 * Giữ UI ổn định qua mọi lần render và không vi phạm purity rules.
 */
const stableNoise = (seed: number) => {
 // NOTE: Avoid Math.sin based PRNG here.
 // Different JS engines / V8 versions can produce slightly different results
 // -> hydration mismatch for inline styles.
 let t = seed + 0x6d2b79f5;
 t = Math.imul(t ^ (t >>> 15), t | 1);
 t ^= t + Math.imul(t ^ (t >>> 7), t | 61);
 return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
};

const getParticleStyle = (index: number) => {
 const top = stableNoise(index + 1) * 100;
 const left = stableNoise(index + 101) * 100;
 const duration = 20 + stableNoise(index + 201) * 35;
 const delay = -stableNoise(index + 301) * 20;

 // Keep strings stable across SSR/CSR (avoid engine differences in float -> string formatting).
 return {
  top: `${top.toFixed(6)}%`,
  left: `${left.toFixed(6)}%`,
  animationDuration: `${duration.toFixed(6)}s`,
  animationDelay: `${delay.toFixed(6)}s`,
 } as const;
};

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

  const purpleBlobClass =
    variant === "subtle"
      ? "absolute top-[-14%] -left-[18%] w-[60vw] h-[60vw] blur-[90px] rounded-full motion-reduce:animate-none"
      : "absolute top-[-20%] -left-1/4 w-[80vw] h-[80vw] blur-[150px] rounded-full motion-reduce:animate-none";

  const mintBlobClass =
    variant === "subtle"
      ? "absolute top-[16%] -right-[18%] w-[52vw] h-[52vw] blur-[80px] rounded-full motion-reduce:animate-none"
      : "absolute top-1/4 -right-1/4 w-[70vw] h-[70vw] blur-[140px] rounded-full motion-reduce:animate-none";

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
 className="fixed inset-0 z-0 pointer-events-none astral-background"
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
        className={`${purpleBlobClass} animate-drift astral-heavy`}
        style={{ backgroundColor: nebulaTone.purple }}
      />
      <div
        className={`${mintBlobClass} animate-drift-reverse astral-heavy`}
        style={{ backgroundColor: nebulaTone.mint }}
      />

 {/* Blob thứ 3 chỉ hiện ở variant default và intense */}
 {variant !== "subtle" && (
        <div
          className="absolute -bottom-1/4 left-1/3 w-[60vw] h-[60vw] blur-[130px] rounded-full animate-slow-pulse motion-reduce:animate-none astral-heavy"
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
 <div className="absolute inset-0 motion-reduce:hidden astral-heavy">
 {Array.from({ length: resolvedParticleCount }).map((_, i) => (
 <div
 key={`astral-particle-${i}`}
 className="absolute w-[2px] h-[2px] bg-[var(--holo-silver)] rounded-full animate-float opacity-[0.4] shadow-[0_0_10px_var(--c-168-156-255-55)]"
 style={getParticleStyle(i)}
 />
 ))}
 </div>
 </div>
 );
}
