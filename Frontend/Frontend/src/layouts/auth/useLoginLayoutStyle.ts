import { theme } from 'antd'
import { useStyleRegister } from '@ant-design/cssinjs'

export const useLoginLayoutStyle = (prefix = 'login-layout') => {
  const { useToken } = theme
  const { token, theme: globalTheme } = useToken()

  useStyleRegister({ theme: globalTheme, token, path: [prefix] }, () => ({
    [`.${prefix}`]: {
      minHeight: '90vh'
    },

    /* Row full height */
    [`.${prefix} .ant-row`]: {
      maxHeight: '90vh'
    },

    /* LEFT IMAGE (Desktop) */
    [`.${prefix} .left`]: {
      height: '100vh',
      overflow: 'hidden'
    },

    [`.${prefix} .left .ant-image`]: {
      width: '100%',
      height: '100%'
    },

    [`.${prefix} .left img`]: {
      width: '100%',
      height: '100%',
      objectFit: 'cover'
    },

    /* RIGHT CONTENT */
    [`.${prefix} .right`]: {
      height: '100vh',
      width: '100%',
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      padding: token.paddingLG,

      '.content': {
        maxWidth: '100%',
        maxHeight: '100vh'
      },
      '.footer': {
        width: '100%',
        textAlign: 'center',
        position: 'absolute',
        bottom: token.marginLG,
        fontSize: token.fontSizeSM,
        color: token.colorTextSecondary
      }
    },

    /* 📱 MOBILE */
    [`@media (max-width: ${token.screenMD}px)`]: {
      [`.${prefix} .left`]: {
        display: 'none'
      },

      [`.${prefix} .right`]: {
        width: '100%',
        height: '100vh'
      }
    }
  }))

  return { prefix }
}
