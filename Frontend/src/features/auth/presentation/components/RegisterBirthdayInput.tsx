"use client";

import { useEffect, useState } from "react";
import { cn } from "@/lib/utils";
import InputFieldMeta from "@/shared/components/ui/input/InputFieldMeta";
import { baseInputStyles } from "@/shared/components/ui/input/inputStyles";
import { Calendar } from "lucide-react";

/**
 * Props cho component RegisterBirthdayInput
 * @property label - Nhãn hiển thị cho ô nhập ngày sinh
 * @property error - Thông báo lỗi nếu có (từ validation)
 * @property value - Giá trị hiện tại của ngày sinh (định dạng YYYY-MM-DD)
 * @property onChange - Callback khi giá trị ngày tháng năm thay đổi
 */
interface RegisterBirthdayInputProps {
  label: string;
  error?: string;
  value?: string;
  onChange: (value: string) => void;
  onBlur?: () => void;
}

/**
 * Component nhập ngày sinh trong một ô duy nhất (Single Box)
 * Tự động thêm dấu `/` khi người dùng nhập đủ chữ số (DD/MM/YYYY)
 * và đồng bộ ngược lại định dạng YYYY-MM-DD cho hệ thống.
 */
export default function RegisterBirthdayInput({ label, error, value, onChange, onBlur }: RegisterBirthdayInputProps) {
  // Giá trị hiển thị định dạng DD/MM/YYYY
  const [displayValue, setDisplayValue] = useState("");

  // Đồng bộ từ giá trị gốc (YYYY-MM-DD) sang định dạng hiển thị (DD/MM/YYYY)
  useEffect(() => {
    if (value && value.includes("-")) {
      const [y, m, d] = value.split("-");
      setDisplayValue(`${d}/${m}/${y}`);
    } else if (!value) {
      setDisplayValue("");
    }
  }, [value]);

  /**
   * Xử lý khi người dùng nhập vào ô text
   * Logic này sẽ tự động thêm dấu "/" và giới hạn ký tự
   */
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const rawValue = e.target.value.replace(/\D/g, ""); // Chỉ lấy số
    const truncatedValue = rawValue.slice(0, 8); // Giới hạn 8 chữ số
    
    let formatted = "";
    if (truncatedValue.length > 0) {
      formatted += truncatedValue.slice(0, 2);
    }
    if (truncatedValue.length > 2) {
      formatted += "/" + truncatedValue.slice(2, 4);
    }
    if (truncatedValue.length > 4) {
      formatted += "/" + truncatedValue.slice(4, 8);
    }

    setDisplayValue(formatted);

    // CHỈ gọi onChange khi đã nhập đủ 8 chữ số để kiểm tra tính hợp lệ
    // Tránh gọi onChange các giá trị dở dang để không trigger validation 'required' sớm
    if (truncatedValue.length === 8) {
      const d = truncatedValue.slice(0, 2);
      const m = truncatedValue.slice(2, 4);
      const y = truncatedValue.slice(4, 8);
      onChange(`${y}-${m}-${d}`);
    }
  };

  /**
   * Xử lý khi người dùng thoát khỏi ô nhập liệu
   */
  const handleBlur = () => {
    // Đồng bộ giá trị cuối cùng về form khi thoát (blur)
    const rawValue = displayValue.replace(/\D/g, "");
    if (rawValue.length === 8) {
      const d = rawValue.slice(0, 2);
      const m = rawValue.slice(2, 4);
      const y = rawValue.slice(4, 8);
      onChange(`${y}-${m}-${d}`);
    } else {
      // Nếu ô trống hoặc nhập dở dang thì truyền rỗng để xóa lỗi hoặc trigger 'required'
      onChange("");
    }
    
    if (onBlur) onBlur();
  };

  // Logic: Chỉ hiển thị viền đỏ và thông báo nếu ô KHÔNG trống
  const showUIError = !!error && displayValue !== "";

  return (
    <div className={cn("flex flex-col w-full")}>
      <InputFieldMeta label={label} renderMeta={false} />
      
      <div className={cn("relative mt-1")}>
        {/* Icon lịch bên trái giống như các Input khác */}
        <div className={cn('pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4 tn-text-secondary')}>
          <Calendar className={cn('h-4 w-4')} />
        </div>
        
        <input
          type="text"
          inputMode="numeric"
          placeholder="DD/MM/YYYY"
          value={displayValue}
          onChange={handleInputChange}
          onBlur={handleBlur}
          className={cn(
            baseInputStyles,
            "pl-11 text-white placeholder:tn-text-muted/50 transition-all",
            showUIError ? "tn-border-danger-50 tn-field-danger" : "tn-field-accent"
          )}
          autoComplete="off"
        />
      </div>

      <InputFieldMeta 
        error={error} 
        isValueEmpty={displayValue === ""} 
        renderLabel={false} 
      />
    </div>
  );
}
