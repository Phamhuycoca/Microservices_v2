import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.min.css'
import './styles/global.scss'
import { StyleProvider } from '@ant-design/cssinjs'
import { ConfigProvider } from 'antd'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import { RequireAuth } from './shared/hooks/useRequireAuth'
import routers from './routers'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <StyleProvider hashPriority='high'>
      <ConfigProvider theme={{ cssVar: { key: 'app' }, hashed: false }}>
        <Router>
          <Routes>
            {routers.map((router) => {
              if (router.protected) {
                return (
                  <Route key={router.path} path={router.path} element={<RequireAuth>{router.component}</RequireAuth>} />
                )
              } else {
                return <Route key={router.path} path={router.path} element={router.component} />
              }
            })}
          </Routes>
        </Router>
      </ConfigProvider>
    </StyleProvider>
  </StrictMode>
)
