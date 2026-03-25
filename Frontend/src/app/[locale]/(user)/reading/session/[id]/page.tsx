import dynamic from "next/dynamic";
import { Suspense } from "react";
import LoadingSpinner from "@/shared/components/ui/LoadingSpinner";

const ReadingSessionPage = dynamic(
 () => import("@/features/reading/session/presentation/ReadingSessionPage"),
 { loading: () => <LoadingSpinner fullPage message="Loading reading session" /> },
);

export default function ReadingSessionRoutePage() {
 return (
  <Suspense fallback={<LoadingSpinner fullPage message="Loading reading session" />}>
   <ReadingSessionPage />
  </Suspense>
 );
}
