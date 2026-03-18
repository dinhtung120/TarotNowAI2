"use client";

/**
 * Input — Component input/textarea thống nhất cho toàn app.
 *
 * === VẤN ĐỀ TRƯỚC ĐÂY ===
 * Mỗi trang viết input styling riêng:
 * - Login: `tn-field tn-field-accent rounded-2xl`
 * - Reading: `tn-field tn-field-accent rounded-2xl`
 * - Wallet: `bg-transparent border-none text-[10px]`
 *
 * === GIẢI PHÁP ===
 * Một component thống nhất với:
 * - Label + Error message tích hợp
 * - Icon trái (prefix icon)
 * - Style consistent qua design tokens
 * - Hỗ trợ textarea mode
 *
 * Tại sao dùng forwardRef?
 * → React Hook Form cần access DOM element qua ref để register field.
 * → Không có ref → `{...register('email')}` không thể gắn vào input.
 */

import { forwardRef, type InputHTMLAttributes, type TextareaHTMLAttributes, type ReactNode } from "react";

/**
 * Props kế thừa HTML input attributes + custom props.
 *
 * Tại sao Omit<..., 'size'>?
 * → HTML input có attribute `size` (số ký tự width) → xung đột với custom size.
 * → Omit loại bỏ HTML size, chỉ giữ custom size.
 */
interface InputProps extends Omit<InputHTMLAttributes<HTMLInputElement>, "size"> {
 /** Label text hiển thị phía trên input */
 label?: string;

 /** Thông báo lỗi — hiển thị đỏ dưới input */
 error?: string;

 /** Hint text — mô tả ngắn dưới input (ẩn khi có error) */
 hint?: string;

 /** Icon bên trái (ví dụ: Mail icon cho email, Lock cho password) */
 leftIcon?: ReactNode;

 /** Chiếm full width container */
 fullWidth?: boolean;
}

/**
 * Props riêng cho textarea mode.
 * Kế thừa HTML textarea attributes thay vì input.
 */
interface TextareaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
 label?: string;
 error?: string;
 hint?: string;
 leftIcon?: ReactNode;
 fullWidth?: boolean;
 /** Đánh dấu đây là textarea, không phải input */
 isTextarea: true;
}

/**
 * Phân biệt Input vs Textarea qua discriminated union.
 * → TypeScript tự biết khi isTextarea=true → props = TextareaProps.
 */
type CombinedProps = (InputProps & { isTextarea?: false }) | TextareaProps;

/**
 * Styling chung cho cả input và textarea.
 * Tuân theo Design System:
 * - Nền sáng dịu (pearl surface)
 * - Viền lavender mềm, rõ hơn khi focus
 * - Focus ring tím-mint nhẹ
 * - Border radius = --radius-md (16px)
 */
const baseInputStyles = [
 "w-full",
 "tn-field",
 "tn-field-accent",
 "rounded-2xl",
 "px-4 py-3",
 "min-h-11",
 "text-sm",
 "disabled:opacity-50 disabled:cursor-not-allowed",
].join(" ");

/**
 * Component Input sử dụng forwardRef để tương thích với React Hook Form.
 *
 * Lưu ý: Không thể dùng forwardRef trực tiếp cho discriminated union
 * (Input vs Textarea) vì ref type khác nhau (HTMLInputElement vs HTMLTextAreaElement).
 * → Giải pháp: Render nội bộ dựa trên isTextarea, forward ref chỉ cho input.
 * → Textarea nhận ref riêng thông qua callback.
 */
const Input = forwardRef<HTMLInputElement, CombinedProps>(
 ({ label, error, hint, leftIcon, fullWidth = true, className = "", ...rest }, ref) => {
 const props = rest as CombinedProps;
 const isTextarea = "isTextarea" in props && props.isTextarea;

 /**
 * Tách isTextarea ra khỏi các props khác để tránh truyền vào DOM element.
 * React sẽ báo lỗi nếu nhận được prop không hợp lệ trên thẻ html (input/textarea).
 */
 const getTextareaDomProps = () => {
 const textareaProps = props as TextareaProps;
 const { isTextarea: isTextareaFlag, ...textareaDomProps } = textareaProps;
 void isTextareaFlag;
 return textareaDomProps;
 };

 return (
 <div className={`space-y-1.5 ${fullWidth ? "w-full" : ""}`}>
 {/* Label — text-[10px] uppercase tracking theo design system */}
 {label && (
 <label className="block text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] ml-1">
 {label}
 </label>
 )}

 {/* Input wrapper — relative container cho left icon positioning */}
 <div className="relative">
 {/* Left icon — absolute positioned bên trái input */}
 {leftIcon && (
 <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none text-[var(--text-secondary)]">
 {leftIcon}
 </div>
 )}

 {isTextarea ? (
 /* Textarea mode — dùng cho câu hỏi, mô tả, bio */
 <textarea
 className={[
 baseInputStyles,
 "resize-none min-h-[80px]",
 leftIcon ? "pl-11" : "",
 error ? "border-[var(--danger)]/50 tn-field-danger" : "",
 className,
 ].join(" ")}
 {...getTextareaDomProps()}
 />
 ) : (
 /* Input mode — text, email, password, number */
 <input
 ref={ref}
 className={[
 baseInputStyles,
 leftIcon ? "pl-11" : "",
 error ? "border-[var(--danger)]/50 tn-field-danger" : "",
 className,
 ].join(" ")}
 {...(props as InputProps)}
 />
 )}
 </div>

 {/* Error message — ưu tiên hiển thị error, nếu không có → hiển thị hint */}
 {error ? (
 <p className="text-[11px] text-[var(--danger)] font-medium ml-1">{error}</p>
 ) : hint ? (
 <p className="text-[11px] text-[var(--text-muted)] font-medium ml-1">{hint}</p>
 ) : null}
 </div>
 );
 },
);

Input.displayName = "Input";

export default Input;
