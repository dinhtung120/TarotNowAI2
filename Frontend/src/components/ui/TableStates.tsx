import type { ReactNode } from 'react';
import { Loader2 } from 'lucide-react';

interface TableStatesProps {
 colSpan: number;
 isLoading: boolean;
 isEmpty: boolean;
 loadingLabel: string;
 emptyLabel: string;
 loadingIcon?: ReactNode;
 emptyIcon?: ReactNode;
}

export default function TableStates({
 colSpan,
 isLoading,
 isEmpty,
 loadingLabel,
 emptyLabel,
 loadingIcon,
 emptyIcon,
}: TableStatesProps) {
 if (isLoading) {
  return (
   <tr>
    <td colSpan={colSpan} className="py-24 text-center">
     <div className="flex flex-col items-center justify-center space-y-4">
      {loadingIcon ?? <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)]" />}
      <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
       {loadingLabel}
      </span>
     </div>
    </td>
   </tr>
  );
 }

 if (isEmpty) {
  return (
   <tr>
    <td colSpan={colSpan} className="py-24 text-center">
     <div className="flex flex-col items-center justify-center space-y-4">
      {emptyIcon}
      <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
       {emptyLabel}
      </span>
     </div>
    </td>
   </tr>
  );
 }

 return null;
}
