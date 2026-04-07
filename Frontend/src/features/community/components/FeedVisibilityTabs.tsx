interface FeedVisibilityTabsProps {
 activeVisibility: string;
 labels: {
  public: string;
  private: string;
 };
 onChange: (visibility: "public" | "private") => void;
}

export function FeedVisibilityTabs({ activeVisibility, labels, onChange }: FeedVisibilityTabsProps) {
 const publicClassName = activeVisibility === "public" ? "border-[#8a2be2] text-[#8a2be2]" : "border-transparent text-gray-500 hover:text-gray-300";
 const privateClassName = activeVisibility === "private" ? "border-[#8a2be2] text-[#8a2be2]" : "border-transparent text-gray-500 hover:text-gray-300";

 return (
  <div className="flex border-b border-[#2a2b3d] mb-6">
   <button type="button" onClick={() => onChange("public")} className={`pb-2 px-4 text-sm font-medium border-b-2 transition-colors ${publicClassName}`}>{labels.public}</button>
   <button type="button" onClick={() => onChange("private")} className={`pb-2 px-4 text-sm font-medium border-b-2 transition-colors ${privateClassName}`}>{labels.private}</button>
  </div>
 );
}
