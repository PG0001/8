"use client";

import { useEffect } from "react";
import { getToken } from "@/lib/auth";
import { useRouter } from "next/navigation";

export default function ProtectedRoute({
  children,
}: {
  children: React.ReactNode;
}) {
  const router = useRouter();

  useEffect(() => {
    if (!getToken()) {
      router.replace("/login"); // ✅ direct & clean
    }
  }, [router]);

  return <>{children}</>;
}
