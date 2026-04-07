import type { ReactNode } from "react";
import { cn } from "@/lib/utils";

interface ChatActionMenuItemButtonProps {
  children: ReactNode;
  className?: string;
  onClick: () => void;
}

export default function ChatActionMenuItemButton({
  children,
  className,
  onClick,
}: ChatActionMenuItemButtonProps) {
  return (
    <button
      className={cn(
        "w-full rounded-lg px-3 py-2 text-left text-xs hover:bg-white/10",
        className,
      )}
      type="button"
      onClick={onClick}
    >
      {children}
    </button>
  );
}
