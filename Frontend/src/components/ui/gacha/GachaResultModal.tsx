'use client';

/* 
 * Import các thành phần hỗ trợ UI và hiệu ứng.
 * - useEffect: Quản lý vòng đời và chuyển cảnh tự động của modal.
 * - Lottie: Hiển thị các hoạt họa mở thưởng sinh động.
 * - GachaResultItem: Thành phần hiển thị phần thưởng đã được tinh chỉnh.
 */
import { memo, useEffect, useState } from 'react';
import Lottie from 'lottie-react';
import Modal from '@/shared/components/ui/Modal';
import Button from '@/shared/components/ui/Button';
import { cn } from '@/lib/utils';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';
import { useRareDropLottie } from '@/shared/infrastructure/gacha/useRareDropLottie';
import { GachaResultItem } from '@/components/ui/gacha/GachaResultItem';

/**
 * Định nghĩa các nhãn ngôn ngữ.
 */
interface GachaResultModalLabels {
  title: string;
  pityTriggered: string;
  close: string;
  rareDropAnimation: string;
}

/**
 * Props của thành phần GachaResultModal.
 */
interface GachaResultModalProps {
  isOpen: boolean;
  locale: string;
  result: PullGachaResult | null;
  labels: GachaResultModalLabels;
  onClose: () => void;
}

/**
 * GachaResultModal - Thành phần trung tâm của trải nghiệm mở thưởng Gacha.
 * Cung cấp quy trình "Mở quà" hồi hộp với hiệu ứng ánh sáng và chuyển cảnh mượt mà.
 */
function GachaResultModalComponent({ isOpen, locale, result, labels, onClose }: GachaResultModalProps) {
  
  /* Trạng thái hiển thị: REVEALING (Đang mở) hoặc SHOW_RESULT (Hiển thị kết quả) */
  const [stage, setStage] = useState<'REVEALING' | 'SHOW_RESULT'>('REVEALING');
  
  /* Lấy dữ liệu hoạt họa Lottie dựa trên phẩm cấp cao nhất của kết quả */
  const { animationData, hasRareDrop } = useRareDropLottie(result?.rewards ?? []);

  /* Reset trạng thái về REVEALING mỗi khi modal được mở lại */
  useEffect(() => {
    if (isOpen) {
      setStage('REVEALING');
      
      /* Tự động chuyển sang trạng thái kết quả sau khi hoạt họa mở được hiển thị xong (~2.5 giây) */
      const timer = setTimeout(() => {
        setStage('SHOW_RESULT');
      }, 2800);
      
      return () => clearTimeout(timer);
    }
  }, [isOpen]);

  if (!result) return null;

  /* Xác định xem có vật phẩm cực hiếm (Legendary/Mythic) để kích hoạt hiệu ứng đặc biệt không */
  const isUltraRare = result.rewards.some(r => 
    String(r.rarity).toLowerCase().includes('5') || 
    String(r.rarity).toLowerCase().includes('legendary') ||
    String(r.rarity).toLowerCase().includes('mythic')
  );

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={onClose} 
      title="" // Không hiển thị tiêu đề modal mặc định để tùy biến giao diện mở thưởng
      size={stage === 'REVEALING' ? 'sm' : 'lg'}
      className={cn(
        'transition-all duration-700',
        isUltraRare && stage === 'REVEALING' ? 'animate-[shake_0.5s_infinite_0.5s]' : ''
      )}
    >
      <div className="relative min-h-[300px] flex flex-col items-center justify-center py-6">
        
        {/* --- Giai đoạn 1: REVEALING (Hoạt họa mở thưởng) --- */}
        {stage === 'REVEALING' && (
          <div className="flex flex-col items-center justify-center space-y-6 animate-in fade-in duration-500">
            {/* Hiệu ứng hào quang nền */}
            <div className={cn(
              'absolute inset-0 -z-10 bg-gradient-to-b from-transparent to-transparent opacity-20 blur-[120px]',
              isUltraRare ? 'from-amber-500' : 'from-purple-500'
            )} />
            
            <div className="relative h-64 w-64">
              {animationData ? (
                <Lottie 
                  animationData={animationData} 
                  loop={false} 
                  autoplay 
                  className="h-full w-full" 
                />
              ) : (
                /* Fallback nếu không có Lottie: Hiển thị vòng sáng nhấp nháy */
                <div className="flex h-64 w-64 items-center justify-center rounded-full bg-gradient-to-tr from-emerald-500/20 via-amber-500/20 to-purple-500/20 animate-pulse">
                   <span className="lunar-metallic-text text-6xl font-black">✦</span>
                </div>
              )}
            </div>
            <p className="lunar-metallic-text text-sm font-black uppercase tracking-[0.3em] animate-pulse">
              Đang khai mở vận mệnh...
            </p>
          </div>
        )}

        {/* --- Giai đoạn 2: SHOW_RESULT (Hiển thị danh sách phần thưởng) --- */}
        {stage === 'SHOW_RESULT' && (
          <div className="w-full space-y-8 animate-in fade-in zoom-in-95 duration-700">
            {/* Tiêu đề kết quả Metallic */}
            <div className="text-center space-y-2">
              <h2 className="lunar-metallic-text text-2xl font-black uppercase tracking-[0.2em]">
                {labels.title}
              </h2>
              <p className="tn-text-secondary text-[10px] font-black uppercase tracking-widest opacity-60">
                {result.poolCode}
              </p>
            </div>

            {/* Thông báo Pity nếu có */}
            {result.wasPityTriggered && (
              <div className="mx-auto max-w-xs animate-bounce">
                <div className="rounded-full bg-amber-500/10 border border-amber-500/30 px-4 py-1.5 text-center">
                   <span className="text-amber-400 text-[10px] font-black uppercase tracking-widest">
                    {labels.pityTriggered}
                   </span>
                </div>
              </div>
            )}

            {/* Danh sách lưới các phần thưởng đã mở được */}
            <div className={cn(
              'grid grid-cols-1 gap-4 overflow-y-auto max-h-[50vh] px-2 custom-scrollbar',
              result.rewards.length > 1 ? 'sm:grid-cols-2' : ''
            )}>
              {result.rewards.map((reward, index) => (
                <GachaResultItem 
                  key={`${reward.itemCode}_${index}`} 
                  reward={reward} 
                  locale={locale} 
                />
              ))}
            </div>

            {/* Nút bấm đóng và hoàn tất */}
            <div className="pt-4">
              <Button 
                variant="brand" 
                size="lg" 
                fullWidth 
                onClick={onClose}
                className="shadow-[0_0_20px_rgba(16,185,129,0.2)]"
              >
                {labels.close}
              </Button>
            </div>
          </div>
        )}
      </div>

      {/* Animation Shake definition cho CSS Inline (nếu cần, nhưng tốt nhất nên dùng trong globals.css) */}
      <style jsx>{`
        @keyframes shake {
          0%, 100% { transform: translate(0, 0); }
          10%, 30%, 50%, 70%, 90% { transform: translate(-2px, -2px); }
          20%, 40%, 60%, 80% { transform: translate(2px, 2px); }
        }
      `}</style>
    </Modal>
  );
}

const GachaResultModal = memo(GachaResultModalComponent);
GachaResultModal.displayName = 'GachaResultModal';

export default GachaResultModal;
