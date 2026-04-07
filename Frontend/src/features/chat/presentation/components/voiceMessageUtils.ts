export const STATIC_BAR_COUNT = 30;

export function generateStaticBars(seed: string): number[] {
 const bars: number[] = [];
 let hash = 0;
 for (let i = 0; i < seed.length; i++) {
  hash = ((hash << 5) - hash + seed.charCodeAt(i)) | 0;
 }

 for (let i = 0; i < STATIC_BAR_COUNT; i++) {
  const value = Math.abs(Math.sin(hash + i * 31) * 10000) % 1;
  bars.push(Math.max(0.15, Math.min(0.9, value)));
 }
 return bars;
}

export function formatDuration(ms: number): string {
 const totalSeconds = Math.max(0, Math.floor(ms / 1000));
 const minutes = Math.floor(totalSeconds / 60);
 const seconds = totalSeconds % 60;
 return `${minutes}:${seconds.toString().padStart(2, '0')}`;
}
