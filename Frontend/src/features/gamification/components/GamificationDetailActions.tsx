'use client';

import { Check } from 'lucide-react';

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
    <div className="mt-10 flex gap-3">
      <button
        onClick={onClose}
        className="flex-1 py-3.5 rounded-2xl bg-slate-800 text-slate-300 font-bold hover:bg-slate-700 transition-colors border border-slate-700/50"
      >
        {closeLabel}
      </button>

      {showClaimButton && (
        <button
          onClick={onClaim}
          disabled={isClaiming}
          className="flex-[2] py-3.5 rounded-2xl bg-gradient-to-r from-indigo-500 to-purple-600 text-white font-black hover:scale-[1.02] active:scale-95 transition-all shadow-lg shadow-indigo-500/25 flex items-center justify-center gap-2"
        >
          {isClaiming ? (
            <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
          ) : (
            <>
              <Check className="w-5 h-5" />
              {claimLabel}
            </>
          )}
        </button>
      )}

      {showEquipButton && (
        <button
          onClick={onEquip}
          className="flex-[2] py-3.5 rounded-2xl bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-black hover:scale-[1.02] active:scale-95 transition-all shadow-lg shadow-blue-500/25 flex items-center justify-center gap-2"
        >
          <Check className="w-5 h-5" />
          {equipLabel}
        </button>
      )}
    </div>
  );
}
