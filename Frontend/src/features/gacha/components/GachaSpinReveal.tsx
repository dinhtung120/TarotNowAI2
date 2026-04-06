"use client";

/*
 * ===================================================================
 * COMPONENT: GachaSpinReveal
 * ===================================================================
 * MỤC ĐÍCH: Hiệu ứng Mở Card/Phần thưởng (Gacha Animation Reveal).
 * Hỗ trợ x10 Pull hiển thị theo dạng Grid.
 */

import { useTranslations } from 'next-intl';
import type { SpinGachaResult, SpinGachaItemResult } from '../gacha.types';
import Modal from '@/shared/components/ui/Modal';
import { useEffect, useState } from 'react';
import Button from '@/shared/components/ui/Button';
import { Diamond, Coins, Award, Sparkles } from 'lucide-react';
import { cn } from '@/shared/utils/cn';
import Image from 'next/image';

interface GachaSpinRevealProps {
  result: SpinGachaResult | null;
  isOpen: boolean;
  onClose: () => void;
}

export function GachaSpinReveal({ result, isOpen, onClose }: GachaSpinRevealProps) {
  const t = useTranslations('gacha');
  const [phase, setPhase] = useState<'closed' | 'firing' | 'revealed'>('closed');

  useEffect(() => {
    if (isOpen && result) {
      setPhase('firing');
      const timer = setTimeout(() => {
        setPhase('revealed');
      }, 2000); // 2s magic pulse then reveal
      return () => clearTimeout(timer);
    } else {
      setPhase('closed');
    }
  }, [isOpen, result]);

  if (!result || !result.items || result.items.length === 0) return null;

  const isVi = t('lang') === 'vi';

  // Component render từng Item
  const ResultItemCard = ({ item, isSingle }: { item: SpinGachaItemResult; isSingle: boolean }) => {
    const name = isVi ? item.displayNameVi : item.displayNameEn;
    
    // Cấu hình màu sắc theo độ hiếm
    const rarityConfig: Record<string, { glow: string; text: string; bg: string; border: string }> = {
      legendary: {
        glow: 'shadow-[0_0_80px_rgba(245,158,11,0.5)]',
        text: 'text-amber-400',
        bg: 'bg-gradient-to-br from-amber-500/20 via-amber-900/40 to-black/60',
        border: 'border-amber-400/60',
      },
      epic: {
        glow: 'shadow-[0_0_60px_rgba(168,85,247,0.4)]',
        text: 'text-purple-400',
        bg: 'bg-gradient-to-br from-purple-500/20 via-purple-900/40 to-black/60',
        border: 'border-purple-400/60',
      },
      rare: {
        glow: 'shadow-[0_0_40px_rgba(59,130,246,0.3)]',
        text: 'text-blue-400',
        bg: 'bg-gradient-to-br from-blue-500/20 via-blue-900/40 to-black/60',
        border: 'border-blue-400/60',
      },
      common: {
        glow: 'shadow-[0_0_20px_rgba(168,162,158,0.2)]',
        text: 'text-stone-300',
        bg: 'bg-stone-800/40',
        border: 'border-stone-700',
      }
    };

    const config = rarityConfig[item.rarity.toLowerCase()] || rarityConfig.common;

    const RewardIcon = ({ sizeClass }: { sizeClass: string }) => {
      if (item.displayIcon) {
        return (
          <img 
            src={`/images/gacha/${item.displayIcon}`} 
            alt={name} 
            className={cn(sizeClass, "object-contain drop-shadow-2xl")} 
            onError={(e) => { 
               e.currentTarget.style.display = 'none'; 
               e.currentTarget.nextElementSibling?.classList.remove('hidden'); 
            }} 
          />
        );
      }
      
      const FallbackIcon = () => {
        const iconProps = { className: cn(sizeClass, config.text) };
        if (item.rewardType === 'diamond') return <Diamond {...iconProps} />;
        if (item.rewardType === 'gold') return <Coins {...iconProps} />;
        if (item.rewardType === 'title') return <Award {...iconProps} />;
        return <Sparkles {...iconProps} />;
      };

      return <FallbackIcon />;
    };

    if (isSingle) {
      return (
        <div className={cn(
          "w-full max-w-2xl min-h-[450px] md:min-h-0 md:h-72 rounded-[2.5rem] p-1 flex flex-col md:flex-row items-center relative border-2 backdrop-blur-xl transition-all animate-in zoom-in-95 duration-700",
          config.glow,
          config.bg,
          config.border
        )}>
          {/* Hình ảnh bên trái - Phóng to */}
          <div className="w-full md:w-1/2 h-64 md:h-full flex items-center justify-center p-8 select-none pointer-events-none">
             <RewardIcon sizeClass="w-full h-full transform scale-125 md:scale-150 transition-transform duration-1000 rotate-3 hover:rotate-0" />
          </div>

          {/* Nội dung bên phải */}
          <div className="w-full md:w-1/2 h-40 md:h-full flex flex-col justify-center px-8 relative">
            {/* Độ hiếm - Góc trên bên phải */}
            <div className="absolute top-4 md:top-8 right-8">
               <span className={cn(
                 "px-4 py-1 rounded-full border text-[10px] uppercase font-black tracking-[0.2em] shadow-inner",
                 config.text,
                 config.border,
                 "bg-black/40"
               )}>
                 {item.rarity}
               </span>
            </div>

            {/* Thông tin giải thưởng - Góc dưới bên phải */}
            <div className="absolute bottom-6 md:bottom-8 right-8 text-right space-y-1">
              <p className="text-[10px] font-black uppercase tracking-[0.3em] opacity-40 text-stone-200">
                Reward Unlocked
              </p>
              <h3 className={cn("text-3xl md:text-4xl font-extrabold italic tracking-tighter uppercase drop-shadow-lg", config.text)}>
                {name}
              </h3>
            </div>
          </div>
        </div>
      );
    }

    // Layout cho x10 (Grid)
    return (
      <div className={cn(
        "rounded-2xl p-4 flex flex-col items-center justify-center relative border backdrop-blur-md animate-in zoom-in-75 duration-300 group",
        config.glow,
        config.bg,
        config.border,
        "w-full aspect-[4/5]"
      )}>
        {/* Rarity small badge */}
        <div className="absolute top-2 right-2">
           <span className={cn("text-[8px] font-black uppercase px-2 py-0.5 rounded-full border bg-black/50", config.text, config.border)}>
             {item.rarity[0]}
           </span>
        </div>
        
        <RewardIcon sizeClass="w-16 h-16 mb-4 group-hover:scale-110 transition-transform duration-500" />
        
        <h3 className={cn("text-center text-[10px] font-bold uppercase tracking-tight line-clamp-1", config.text)}>
          {name}
        </h3>
      </div>
    );
  };


  return (
    <Modal 
      isOpen={isOpen} 
      onClose={() => {
        if (phase === 'revealed') onClose();
      }}
      title={t('spinResult')}
      size="lg"
      showCloseButton={phase === 'revealed'}
    >
      <div className={cn(
        "flex flex-col items-center justify-center min-h-[400px]",
        result.items.length > 1 ? "w-full" : "max-w-2xl mx-auto"
      )}>
        {phase === 'firing' && (
          <div className="flex flex-col items-center justify-center animate-pulse">
            <div className="w-24 h-24 rounded-full border-4 border-indigo-500 border-t-transparent animate-spin mb-6" />
            <p className="text-xl font-black text-indigo-400 tracking-widest uppercase">{t('revealing')}...</p>
          </div>
        )}

        {phase === 'revealed' && (
          <div className="w-full h-full flex flex-col items-center">
            {result.wasPityTriggered && (
              <p className="text-amber-500 text-xs mt-2 mb-6 font-black bg-amber-500/10 px-4 py-2 rounded-full border border-amber-500/20 uppercase tracking-widest pulse">
                🌟 {t('pityTriggered')}
              </p>
            )}

            <div className={cn(
              "w-full",
              result.items.length > 1 
                ? "grid grid-cols-2 md:grid-cols-5 gap-4" 
                : "flex justify-center"
            )}>
              {result.items.map((item, idx) => (
                <ResultItemCard key={idx} item={item} isSingle={result.items.length === 1} />
              ))}
            </div>


            <Button 
              variant="brand"
              size="lg"
              className="mt-10 min-w-xs"
              onClick={onClose}
            >
              {t('awesome')}
            </Button>
          </div>
        )}
      </div>
    </Modal>
  );
}
