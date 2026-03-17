/**
 * Barrel export — UI Components
 *
 * Tại sao cần barrel export (index.ts)?
 * → Thay vì import từng file riêng lẻ:
 * import Button from '@/components/ui/Button';
 * import Badge from '@/components/ui/Badge';
 * import GlassCard from '@/components/ui/GlassCard';
 *
 * → Chỉ cần 1 dòng:
 * import { Button, Badge, GlassCard } from '@/components/ui';
 *
 * → Gọn hơn, dễ quản lý import, IDE auto-complete tốt hơn.
 */

export { default as Button } from "./Button";
export { default as Input } from "./Input";
export { default as GlassCard } from "./GlassCard";
export { default as Badge } from "./Badge";
export { default as Modal } from "./Modal";
export { default as LoadingSpinner } from "./LoadingSpinner";
export { default as SkeletonLoader } from "./SkeletonLoader";
export { default as EmptyState } from "./EmptyState";
export { default as SectionHeader } from "./SectionHeader";
export { default as Pagination } from "./Pagination";
