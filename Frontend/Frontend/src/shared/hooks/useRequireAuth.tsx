import { Navigate } from 'react-router-dom'
import { useIsAuthen } from './useIsAuthen'
import type { ReactNode } from 'react'

interface RequireAuthProps {
  children: ReactNode
}

export function RequireAuth({ children }: RequireAuthProps): ReactNode | null {
  const { isAuthen, loading } = useIsAuthen()

  if (loading) return null

  return isAuthen ? children : <Navigate to='/login' replace />
}
