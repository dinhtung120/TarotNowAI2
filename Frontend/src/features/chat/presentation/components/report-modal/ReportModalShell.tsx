import type { MouseEvent, ReactNode } from 'react';
import { cn } from '@/lib/utils';

interface ReportModalShellProps {
  children: ReactNode;
  onClose: () => void;
  panelClassName?: string;
}

export default function ReportModalShell({ children, onClose, panelClassName }: ReportModalShellProps) {
  return (
    <div className={cn('fixed inset-0 z-50 flex items-center justify-center bg-black/70')} onClick={onClose}>
      <div className={cn('mx-4 w-full max-w-md rounded-3xl border border-white/10 bg-black/85 p-8', panelClassName)} onClick={(event: MouseEvent<HTMLDivElement>) => event.stopPropagation()}>
        {children}
      </div>
    </div>
  );
}
