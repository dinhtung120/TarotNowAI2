/*
 * ===================================================================
 * COMPONENT/FILE: UI Barrel Export (index.ts)
 * BỐI CẢNH (CONTEXT):
 *   Tập hợp phân phối (Barrel) cho toàn bộ UI Components tĩnh của ứng dụng.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Gom nhóm các UI Components (Button, Input, Modal, v.v.) vào một đầu mối import duy nhất.
 *   - Giúp các file khác khai báo import gọn gàng: `import { Button, Input } from "@/components/ui"` 
 *     thay vì phải import lẻ từng file.
 * ===================================================================
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
export { default as TableStates } from "./TableStates";
export { default as FilterTabs } from "./FilterTabs";
export { default as ActionConfirmModal } from "./ActionConfirmModal";
export { default as StepPagination } from "./StepPagination";
