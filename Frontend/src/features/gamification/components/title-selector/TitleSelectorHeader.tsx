import { Medal } from "lucide-react";

interface TitleSelectorHeaderProps {
 title: string;
 subtitle: string;
}

export function TitleSelectorHeader({ title, subtitle }: TitleSelectorHeaderProps) {
 return (
  <div className="flex items-center gap-3 mb-6 relative z-10">
   <div className="p-3 bg-gradient-to-br from-blue-400 to-indigo-600 rounded-xl shadow-lg shadow-blue-500/30">
    <Medal className="w-6 h-6 text-white" />
   </div>
   <div>
    <h2 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-100 to-indigo-200">{title}</h2>
    <p className="text-sm text-blue-200/70">{subtitle}</p>
   </div>
  </div>
 );
}
