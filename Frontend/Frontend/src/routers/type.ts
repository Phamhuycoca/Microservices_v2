import type { ReactNode } from "react";

export interface RouterType {
    path: string;
    component: ReactNode;
    protected?: boolean | false;
}

