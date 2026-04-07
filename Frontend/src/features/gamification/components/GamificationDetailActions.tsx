'use client';

import { Check } from 'lucide-react';
import { cn } from '@/lib/utils';

interface GamificationDetailActionsProps {
  showClaimButton: boolean;
  showEquipButton: boolean;
  onClose: () => void;
  onClaim?: () => void;
  onEquip?: () => void;
  isClaiming?: boolean;
  closeLabel: string;
  claimLabel: string;
  equipLabel: string;
}

export function GamificationDetailActions({
  showClaimButton,
  showEquipButton,
  onClose,
  onClaim,
  onEquip,
  isClaiming,
  closeLabel,
  claimLabel,
  equipLabel,
}: GamificationDetailActionsProps) {
  return (
    <div className={cn("mt-10 flex gap-3")}>
      <button
        type="button"
        onClick={onClose}
        className={cn("flex-1 py-3.5 rounded-2xl bg-slate-800 text-slate-300 font-bold transition-colors border border-slate-700/50")}
      >
        {closeLabel}
      </button>

      {showClaimButton && (
        <button
          type="button"
          onClick={onClaim}
          disabled={isClaiming}
          className={cn("tn-flex-2 py-3.5 rounded-2xl bg-gradient-to-r from-indigo-500 to-purple-600 text-white font-black transition-all shadow-lg shadow-indigo-500/25 flex items-center justify-center gap-2")}
        >
          {isClaiming ? (
            <div className={cn("w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin")} />
          ) : (
            <>
              <Check className={cn("w-5 h-5")} />
              {claimLabel}
            </>
          )}
        </button>
      )}

      {showEquipButton && (
        <button
          type="button"
          onClick={onEquip}
          className={cn("tn-flex-2 py-3.5 rounded-2xl bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-black transition-all shadow-lg shadow-blue-500/25 flex items-center justify-center gap-2")}
        >
          <Check className={cn("w-5 h-5")} />
          {equipLabel}
        </button>
      )}
    </div>
  );
}
