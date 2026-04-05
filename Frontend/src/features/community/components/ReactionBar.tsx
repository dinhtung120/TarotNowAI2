/*
 * ===================================================================
 * FILE: ReactionBar.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thanh công cụ để chọn Emoji Reaction.
 * ===================================================================
 */

import React from 'react';
import { ReactionType } from '../types';
import { useToggleReaction } from '../hooks/useToggleReaction';

interface ReactionBarProps {
  postId: string;
  reactionsCount: Record<string, number>;
  viewerReaction: ReactionType | null;
  currentVisibilityTab?: string;
}

const REACTIONS_MAP: Record<ReactionType, string> = {
  like: '👍',
  love: '❤️',
  insightful: '💡',
  haha: '😄',
  sad: '😢',
};

export const ReactionBar: React.FC<ReactionBarProps> = ({ 
  postId, 
  reactionsCount, 
  viewerReaction,
  currentVisibilityTab
}) => {
  const toggleReaction = useToggleReaction({ postId, visibility: currentVisibilityTab });

  const handleReact = (type: ReactionType) => {
    toggleReaction.mutate(type);
  };

  return (
    <div className="flex flex-wrap gap-2 pt-3 border-t border-[#2a2b3d]">
      {(Object.entries(REACTIONS_MAP) as [ReactionType, string][]).map(([type, emoji]) => {
        const count = reactionsCount[type] || 0;
        const isActive = viewerReaction === type;

        // Chỉ ưu tiên hiện nếu nó có người bấm, TRỪ khi viewerReaction đang tick vào nó. (Để UI gọn).
        // Tuy nhiên thường Reaction bar sẽ giấu icon, khi hover mới hiện popover. 
        // Trong MVP Phase 4.1 này, ta in thẳng nút ra với opacity để đơn giản.
        return (
          <button
            key={type}
            onClick={() => handleReact(type)}
            className={`
              flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium transition-all
              ${isActive 
                ? 'bg-[#8a2be2]/20 text-[#8a2be2] border border-[#8a2be2]/30' 
                : 'bg-transparent text-gray-400 hover:bg-[#2a2b3d] border border-transparent'}
            `}
          >
            <span className="text-sm">{emoji}</span>
            {count > 0 && <span>{count}</span>}
          </button>
        );
      })}
    </div>
  );
};
