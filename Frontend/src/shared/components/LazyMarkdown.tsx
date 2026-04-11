'use client';

import dynamic from 'next/dynamic';
import { cn } from '@/lib/utils';

const ReactMarkdown = dynamic(() => import('react-markdown'), {
 ssr: true,
 loading: () => <span className={cn('inline-block', 'animate-pulse', 'text-slate-500')}>…</span>,
});

type LazyMarkdownProps = {
 children: string;
};

export function LazyMarkdown({ children }: LazyMarkdownProps) {
 return <ReactMarkdown>{children}</ReactMarkdown>;
}
