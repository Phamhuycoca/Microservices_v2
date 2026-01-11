export function useIsAuthen() {
  const token = localStorage.getItem('access_token')

  return {
    isAuthen: !!token,
    loading: false
  }
}
