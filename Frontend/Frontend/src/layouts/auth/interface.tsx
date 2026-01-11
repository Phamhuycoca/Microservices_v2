interface AdminLayoutInterface {
  image: string
  title: string
}
type FieldType = {
  username?: string
  password?: string
  remember?: string
}
export type { AdminLayoutInterface, FieldType }
