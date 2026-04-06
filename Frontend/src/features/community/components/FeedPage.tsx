 'use client';

/*
 * ===================================================================
 * FILE: FeedPage.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trang chứa bảng tin (Feed) tổng hợp nơi hiển thị Composer và List bài.
 * ===================================================================
 */

import React, { useState } from 'react';
import { useTranslations } from 'next-intl';
import { useFeed } from '../hooks/useFeed';
import { PostComposer } from './PostComposer';
import { PostCard } from './PostCard';
// Tạm thời bỏ đi Modal Report thực, sẽ mock logic.

export const FeedPage: React.FC = () => {
  const t = useTranslations('Community');
  const [activeVisibility, setActiveVisibility] = useState<string>('public');
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading } = useFeed(activeVisibility);

  const handleReport = (postId: string) => {
    alert(t('feed.report_placeholder', { postId }));
  };

  return (
    <div className="w-full max-w-2xl mx-auto py-8 px-4">
      <h1 className="text-3xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-purple-400 to-pink-500 mb-6 font-serif">
        {t('title')}
      </h1>

      <PostComposer currentVisibilityTab={activeVisibility} />

      {/* Tabs mỏng giấu trên */}
      <div className="flex border-b border-[#2a2b3d] mb-6">
        <button 
          onClick={() => setActiveVisibility('public')}
          className={`pb-2 px-4 text-sm font-medium border-b-2 transition-colors ${
            activeVisibility === 'public' 
            ? 'border-[#8a2be2] text-[#8a2be2]' 
            : 'border-transparent text-gray-500 hover:text-gray-300'
          }`}
        >
          {t('tabs.public')}
        </button>
        <button 
          onClick={() => setActiveVisibility('private')}
          className={`pb-2 px-4 text-sm font-medium border-b-2 transition-colors ${
            activeVisibility === 'private' 
            ? 'border-[#8a2be2] text-[#8a2be2]' 
            : 'border-transparent text-gray-500 hover:text-gray-300'
          }`}
        >
          {t('tabs.private')}
        </button>
      </div>

      {isLoading ? (
        <div className="text-center text-gray-500 py-10">{t('feed.loading')}</div>
      ) : (
        <div className="space-y-4">
          {data?.pages.map((page, i) => (
            <React.Fragment key={i}>
              {page.data.map((post) => (
                <PostCard 
                  key={post.id} 
                  post={post} 
                  currentVisibilityTab={activeVisibility} 
                  onReportClick={handleReport} 
                />
              ))}
            </React.Fragment>
          ))}

          {/* Nút Load More tạm thời thay thế Intersection Observer cho dễ test */}
          {hasNextPage && (
            <button
              onClick={() => fetchNextPage()}
              disabled={isFetchingNextPage}
              className="w-full py-3 mt-4 text-sm text-[#8a2be2] border border-[#2a2b3d] rounded-xl hover:bg-[#2a2b3d]/50 transition-colors"
            >
              {isFetchingNextPage ? t('feed.loading_more') : t('feed.load_more')}
            </button>
          )}

          {!hasNextPage && data?.pages[0].data.length !== 0 && (
            <div className="text-center text-gray-600 py-6 text-sm">
              {t('feed.end')}
            </div>
          )}

          {data?.pages[0].data.length === 0 && (
            <div className="text-center text-gray-500 py-10 border border-dashed border-[#2a2b3d] rounded-xl">
              {t('feed.empty')}
            </div>
          )}
        </div>
      )}
    </div>
  );
};
