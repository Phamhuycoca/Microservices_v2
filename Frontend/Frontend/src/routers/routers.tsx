import { AdminLayout, AppLayout, LoginLayout } from '@/layouts'
import type { RouterType } from './type'

export const routers: RouterType[] = [
  {
    path: '/',
    component: <AppLayout />,
    protected: false
  },
  {
    path: '/admin',
    component: <AdminLayout />,
    protected: true
  },
  {
    path: '/login',
    component: <LoginLayout />,
    protected: false
  }
]
