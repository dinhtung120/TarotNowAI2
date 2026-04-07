export interface GachaRarityStyle {
 bg: string;
 border: string;
 glow: string;
 text: string;
}

export const RARITY_CONFIG: Record<string, GachaRarityStyle> = {
 legendary: {
  glow: 'shadow-[0_0_80px_rgba(245,158,11,0.5)]',
  text: 'text-amber-400',
  bg: 'bg-gradient-to-br from-amber-500/20 via-amber-900/40 to-black/60',
  border: 'border-amber-400/60',
 },
 epic: {
  glow: 'shadow-[0_0_60px_rgba(168,85,247,0.4)]',
  text: 'text-purple-400',
  bg: 'bg-gradient-to-br from-purple-500/20 via-purple-900/40 to-black/60',
  border: 'border-purple-400/60',
 },
 rare: {
  glow: 'shadow-[0_0_40px_rgba(59,130,246,0.3)]',
  text: 'text-blue-400',
  bg: 'bg-gradient-to-br from-blue-500/20 via-blue-900/40 to-black/60',
  border: 'border-blue-400/60',
 },
 common: {
  glow: 'shadow-[0_0_20px_rgba(168,162,158,0.2)]',
  text: 'text-stone-300',
  bg: 'bg-stone-800/40',
  border: 'border-stone-700',
 },
};
