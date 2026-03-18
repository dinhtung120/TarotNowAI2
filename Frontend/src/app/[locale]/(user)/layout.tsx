import type { ReactNode } from "react";
import UserLayout from "@/components/layout/UserLayout";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

export default function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  return <UserLayout>{children}</UserLayout>;
}
