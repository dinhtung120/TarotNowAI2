export type AstralVariant = "subtle" | "default" | "intense";

export function getResolvedParticleCount(variant: AstralVariant, particleCount?: number) {
 return particleCount ?? (variant === "subtle" ? 8 : variant === "intense" ? 25 : 15);
}

export function getNebulaTone(variant: AstralVariant) {
 if (variant === "subtle") {
  return {
   purple: "var(--nebula-purple-subtle)",
   mint: "var(--nebula-mint-subtle)",
   moon: "var(--nebula-moon-subtle)",
  };
 }

 if (variant === "intense") {
  return {
   purple: "var(--nebula-purple-intense)",
   mint: "var(--nebula-mint-intense)",
   moon: "var(--nebula-moon-intense)",
  };
 }

 return {
  purple: "var(--nebula-purple-default)",
  mint: "var(--nebula-mint-default)",
  moon: "var(--nebula-moon-default)",
 };
}

function stableNoise(seed: number) {
 let t = seed + 0x6d2b79f5;
 t = Math.imul(t ^ (t >>> 15), t | 1);
 t ^= t + Math.imul(t ^ (t >>> 7), t | 61);
 return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
}

export function getParticleStyle(index: number) {
 const top = stableNoise(index + 1) * 100;
 const left = stableNoise(index + 101) * 100;
 const duration = 20 + stableNoise(index + 201) * 35;
 const delay = -stableNoise(index + 301) * 20;

 return {
  top: `${top.toFixed(6)}%`,
  left: `${left.toFixed(6)}%`,
  animationDuration: `${duration.toFixed(6)}s`,
  animationDelay: `${delay.toFixed(6)}s`,
 } as const;
}
