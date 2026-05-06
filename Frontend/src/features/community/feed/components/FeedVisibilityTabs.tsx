import { cn } from "@/lib/utils";

interface FeedVisibilityTabsProps {
 activeVisibility: string;
 labels: {
  public: string;
  private: string;
 };
 onChange: (visibility: "public" | "private") => void;
}

export function FeedVisibilityTabs({ activeVisibility, labels, onChange }: FeedVisibilityTabsProps) {
 const publicClassName = activeVisibility === "public" ? cn("border-violet-500", "text-violet-400") : cn("border-transparent", "text-gray-500");
 const privateClassName = activeVisibility === "private" ? cn("border-violet-500", "text-violet-400") : cn("border-transparent", "text-gray-500");

 return (
  <div className={cn("mb-6", "flex", "border-b", "border-slate-700")}>
   <button type="button" onClick={() => onChange("public")} className={cn("border-b-2", "px-4", "pb-2", "text-sm", "font-medium", "transition-colors", publicClassName)}>{labels.public}</button>
   <button type="button" onClick={() => onChange("private")} className={cn("border-b-2", "px-4", "pb-2", "text-sm", "font-medium", "transition-colors", privateClassName)}>{labels.private}</button>
  </div>
 );
}
