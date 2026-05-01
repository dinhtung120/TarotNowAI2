import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { cn } from '@/lib/utils';
import { Star, Sparkles, Wand2 } from 'lucide-react';

/**
 * Component render Markdown cho kết quả trả bài Tarot.
 * Được thiết kế để mang lại cảm giác cao cấp (premium) và huyền bí.
 * 
 * @param {Object} props - Thuộc tính của component.
 * @param {string} props.content - Nội dung Markdown cần render.
 * @param {string} [props.className] - CSS class bổ sung.
 */
export function ReadingMarkdownRenderer({ content, className }: { content: string; className?: string }) {
  return (
    <div className={cn("max-w-none text-left", className)}>
      <ReactMarkdown
        remarkPlugins={[remarkGfm]}
        components={{
          // Tùy chỉnh cách hiển thị tiêu đề (h4 thường dùng cho vị trí lá bài)
          h4: ({ node, ...props }) => (
            <div className="mt-10 mb-6 group animate-in fade-in slide-in-from-left-4 duration-700">
              <div className="flex items-center gap-4 mb-3">
                <div className="relative">
                  <div className="absolute -inset-1 bg-gradient-to-r from-purple-600 to-pink-600 rounded-full blur opacity-25 group-hover:opacity-50 transition duration-1000 group-hover:duration-200"></div>
                  <div className="relative flex h-10 w-10 items-center justify-center rounded-full tn-bg-accent-10 border tn-border-accent-20 group-hover:scale-110 transition-transform">
                    <Star className="h-5 w-5 tn-text-accent fill-current" />
                  </div>
                </div>
                <div className="flex flex-col">
                  <h4 className="text-xl font-black tracking-tight tn-text-primary m-0 uppercase italic leading-none" {...props} />
                  <div className="h-1 w-24 mt-1 bg-gradient-to-r from-purple-500 to-transparent rounded-full" />
                </div>
              </div>
            </div>
          ),
          // Tùy chỉnh cách hiển thị văn bản in đậm
          strong: ({ node, ...props }) => (
            <strong 
              className="tn-text-accent font-extrabold italic px-0.5 bg-gradient-to-t from-purple-500/10 to-transparent rounded-sm" 
              {...props} 
            />
          ),
          // Tùy chỉnh cách hiển thị đoạn văn
          p: ({ node, ...props }) => {
             // Kiểm tra nếu là đoạn văn mở đầu hoặc kết thúc để style khác đi một chút
             const text = props.children?.toString() || '';
             const isHighlight = text.length < 200 && (text.includes('Chào') || text.includes('Tóm lại') || text.includes('Cảm ơn'));
             
             return (
               <p 
                 className={cn(
                   "tn-text-secondary leading-relaxed mb-6 text-[16px] animate-in fade-in duration-1000",
                   isHighlight ? "tn-text-primary font-medium italic border-l-2 tn-border-accent-20 pl-4 py-1" : ""
                 )} 
                 {...props} 
               />
             );
          },
          // Tùy chỉnh danh sách
          ul: ({ node, ...props }) => (
            <ul className="list-none p-0 space-y-3 mb-8 ml-4" {...props} />
          ),
          li: ({ node, ...props }) => (
            <li className="flex items-start gap-3 tn-text-secondary group" {...props}>
              <div className="mt-2.5 h-1.5 w-1.5 rounded-full tn-bg-accent shrink-0 group-hover:scale-150 transition-transform" />
              <span className="group-hover:tn-text-primary transition-colors">{props.children}</span>
            </li>
          ),
          // Tùy chỉnh blockquote
          blockquote: ({ node, ...props }) => (
            <div className="relative my-8 animate-in zoom-in-95 duration-500">
              <div className="absolute -left-2 top-0 bottom-0 w-1 bg-gradient-to-b from-purple-600 to-pink-600 rounded-full" />
              <blockquote className="tn-bg-glass-strong border tn-border-soft px-8 py-6 rounded-2xl italic tn-text-primary shadow-inner" {...props} />
              <Wand2 className="absolute -right-3 -top-3 h-6 w-6 tn-text-accent opacity-50 rotate-12" />
            </div>
          ),
          // Tùy chỉnh hr
          hr: ({ node, ...props }) => (
            <div className="my-12 flex items-center justify-center gap-4 opacity-30">
              <div className="h-px w-full bg-gradient-to-r from-transparent to-purple-500" />
              <Sparkles className="h-5 w-5 shrink-0" />
              <div className="h-px w-full bg-gradient-to-l from-transparent to-purple-500" />
            </div>
          )
        }}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
}
