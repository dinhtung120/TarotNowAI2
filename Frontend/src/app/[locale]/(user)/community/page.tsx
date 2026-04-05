/*
 * ===================================================================
 * FILE: page.tsx
 * NAMESPACE: src/app/[locale]/community
 * ===================================================================
 * MỤC ĐÍCH:
 *   Next.js App Router đâm thẳng vào Mạng Xã Hội TarotNow.
 * ===================================================================
 */

'use client';

import React from 'react';
import { FeedPage } from '@/features/community/components/FeedPage';

export default function CommunityIndexPage() {
  return (
    <main className="min-h-screen bg-[#0f0f16] text-white">
      <FeedPage />
    </main>
  );
}
