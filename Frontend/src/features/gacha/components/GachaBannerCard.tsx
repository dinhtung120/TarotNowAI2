"use client";

/*
 * ===================================================================
 * COMPONENT: GachaBannerCard
 * ===================================================================
 * MỤC ĐÍCH: Hiển thị một banner Gacha, số diamond cần thiết và tiến trình Pity.
 * Sử dụng GlassCard và Button từ hệ thống Shared UI.
 */

import { useTranslations } from 'next-intl';
import type { GachaBannerDto } from '../gacha.types';
import Button from '@/shared/components/ui/Button';
import GlassCard from '@/shared/components/ui/GlassCard';
import { Diamond, Sparkles, HelpCircle } from 'lucide-react';
import { useState } from 'react';
import { GachaOddsModal } from './GachaOddsModal';
import { cn } from '@/shared/utils/cn';

interface GachaBannerCardProps {
  banner: GachaBannerDto;
  onSpin: (bannerCode: string, count: number) => void;
  isSpinning: boolean;
  currentPity: number;
  hardPityCount: number;
}

export function GachaBannerCard({ banner, onSpin, isSpinning, currentPity, hardPityCount }: GachaBannerCardProps) {
  const t = useTranslations('gacha');
  const [isOddsOpen, setIsOddsOpen] = useState(false);

  const isVi = t('lang') === 'vi';
  const name = isVi ? banner.nameVi : banner.nameEn;
  const description = isVi ? banner.descriptionVi : banner.descriptionEn;

  // Tính toán phần trăm Pity
  const pityPercentage = Math.min(Math.max((currentPity / hardPityCount) * 100, 2), 100);

  return (
    <GlassCard 
      variant="interactive" 
      padding="none" 
      className="w-full max-w-md bg-stone-900 border-stone-800 text-stone-100 overflow-hidden relative group flex flex-col"
    >
      {/* Decorative gradient background */}
      <div className="absolute inset-0 bg-gradient-to-br from-indigo-900/20 via-purple-900/10 to-transparent opacity-50 group-hover:opacity-100 transition-opacity duration-500 pointer-events-none" />
      
      <div className="p-6 relative z-10">
        <div className="flex justify-between items-start">
          <h3 className="text-xl font-black uppercase tracking-widest bg-clip-text text-transparent bg-gradient-to-r from-purple-400 to-indigo-400">
            {name}
          </h3>
          <button 
            onClick={() => setIsOddsOpen(true)}
            className="p-1.5 rounded-lg bg-stone-800/50 text-stone-400 hover:text-stone-200 transition-colors border border-stone-700/50"
            title={t('viewOdds')}
          >
            <HelpCircle className="w-4 h-4" />
          </button>
        </div>
        <p className="text-[11px] font-medium text-stone-400 mt-2 uppercase tracking-tight leading-relaxed line-clamp-2">
          {description}
        </p>
      </div>

      <div className="px-6 pb-6 relative z-10 space-y-6 flex-1">
        {/* Pity Progress */}
        <div className="space-y-3">
          <div className="flex justify-between text-[10px] uppercase font-black text-stone-500 tracking-widest">
            <span>{t('pityProgress')}</span>
            <span className="text-stone-300">{currentPity} / {hardPityCount}</span>
          </div>
          
          {/* Custom Progress Bar */}
          <div className="h-2 w-full bg-stone-950 rounded-full overflow-hidden border border-stone-800/50 p-[1px]">
            <div 
              className={cn(
                "h-full rounded-full transition-all duration-1000 ease-out shadow-[0_0_8px_rgba(168,85,247,0.4)]",
                pityPercentage > 90 ? "bg-amber-400" : "bg-gradient-to-r from-indigo-500 to-purple-500"
              )}
              style={{ width: `${pityPercentage}%` }}
            />
          </div>
          
          <p className="text-[9px] font-bold uppercase tracking-wide text-stone-600 text-right">
            {t('guaranteedLegendary')}
          </p>
        </div>
      </div>

      <div className="px-6 pb-6 relative z-10 flex gap-3">
        <Button 
          variant="primary"
          size="md"
          className="flex-1 flex flex-col items-center !min-h-[60px]"
          onClick={() => onSpin(banner.code, 1)}
          disabled={isSpinning}
          isLoading={isSpinning}
        >
          {!isSpinning && (
            <div className="flex flex-col items-center py-1">
              <span className="flex items-center gap-1"><Sparkles className="w-3 h-3" /> 1x</span>
              <span className="flex items-center text-amber-400 text-[10px] font-black mt-0.5">
                {banner.costDiamond} <Diamond className="w-2.5 h-2.5 ml-1 fill-amber-400" />
              </span>
            </div>
          )}
        </Button>

        <Button 
          variant="brand"
          size="md"
          className="flex-1 flex flex-col items-center !min-h-[60px]"
          onClick={() => onSpin(banner.code, 10)}
          disabled={isSpinning}
        >
          {isSpinning ? (
             <span className="animate-shimmer">{t('spinning')}</span>
          ) : (
             <div className="flex flex-col items-center py-1">
              <span className="flex items-center gap-1"><Sparkles className="w-3 h-3" /> 10x</span>
              <span className="flex items-center text-stone-900 text-[10px] font-black mt-0.5">
                {banner.costDiamond * 10} <Diamond className="w-2.5 h-2.5 ml-1 fill-stone-900" />
              </span>
            </div>
          )}
        </Button>
      </div>

      {isOddsOpen && (
        <GachaOddsModal 
          bannerCode={banner.code} 
          isOpen={isOddsOpen} 
          onClose={() => setIsOddsOpen(false)} 
        />
      )}
    </GlassCard>
  );
}
