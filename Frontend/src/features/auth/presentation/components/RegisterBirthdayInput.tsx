"use client";

import { useMemo, useState } from "react";
import { cn } from "@/lib/utils";
import InputFieldMeta from "@/shared/components/ui/input/InputFieldMeta";
import { baseInputStyles } from "@/shared/components/ui/input/inputStyles";
import { Calendar } from "lucide-react";
import {
 formatBirthdayDraft,
 formatIsoToDisplay,
 parseDisplayBirthday,
} from "@/features/auth/presentation/components/registerBirthdayInputValue";

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
    const formatted = formatBirthdayDraft(event.target.value);
    setDraftValue(formatted);
    const normalized = parseDisplayBirthday(formatted);
    if (normalized) {
      onChange(normalized);
    }
  };

  const handleBlur = () => {
    onChange(parseDisplayBirthday(displayValue));

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
