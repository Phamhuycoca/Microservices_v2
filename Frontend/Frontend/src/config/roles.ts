export const ROLE = {
  ADMIN: 'ADMIN',
  USER: 'USER',
  MANAGER: 'MANAGER',
} as const;
export type ROLE = typeof ROLE[keyof typeof ROLE];