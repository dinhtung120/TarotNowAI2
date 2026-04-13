"use client";

import { useMemo, useState } from "react";
import { cn } from "@/lib/utils";
import InputFieldMeta from "@/shared/components/ui/input/InputFieldMeta";
import { baseInputStyles } from "@/shared/components/ui/input/inputStyles";
import { Calendar } from "lucide-react";

interface RegisterBirthdayInputProps {
  label: string;
  error?: string;
  value?: string;
  onChange: (value: string) => void;
  onBlur?: () => void;
}

export default function RegisterBirthdayInput({ label, error, value, onChange, onBlur }: RegisterBirthdayInputProps) {
  const [draftValue, setDraftValue] = useState<string | null>(null);

  const displayValue = useMemo(() => {
    if (draftValue !== null) {
      return draftValue;
    }

    return formatIsoToDisplay(value);
  }, [draftValue, value]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const rawValue = event.target.value.replace(/\D/g, "").slice(0, 8);

    let formatted = "";
    if (rawValue.length > 0) {
      formatted += rawValue.slice(0, 2);
    }
    if (rawValue.length > 2) {
      formatted += `/${rawValue.slice(2, 4)}`;
    }
    if (rawValue.length > 4) {
      formatted += `/${rawValue.slice(4, 8)}`;
    }

    setDraftValue(formatted);

    if (rawValue.length === 8) {
      const day = rawValue.slice(0, 2);
      const month = rawValue.slice(2, 4);
      const year = rawValue.slice(4, 8);
      onChange(`${year}-${month}-${day}`);
    }
  };

  const handleBlur = () => {
    const rawValue = displayValue.replace(/\D/g, "");
    if (rawValue.length === 8) {
      const day = rawValue.slice(0, 2);
      const month = rawValue.slice(2, 4);
      const year = rawValue.slice(4, 8);
      onChange(`${year}-${month}-${day}`);
    } else {
      onChange("");
    }

    setDraftValue(null);
    onBlur?.();
  };

  const showUIError = !!error && displayValue !== "";

  return (
    <div className={cn("flex flex-col w-full")}>
      <InputFieldMeta label={label} renderMeta={false} />

      <div className={cn("relative mt-1")}>
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

function formatIsoToDisplay(value?: string): string {
  if (!value || !value.includes("-")) {
    return "";
  }

  const [year, month, day] = value.split("-");
  return `${day}/${month}/${year}`;
}
