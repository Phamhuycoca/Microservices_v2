import { Button, Checkbox, Col, Divider, Form, Image, Input, Row, type FormProps } from 'antd'
import { useLoginLayoutStyle } from './useLoginLayoutStyle'
import { useState } from 'react'
import type { AdminLayoutInterface, FieldType } from './interface'
import { Link, useNavigate } from 'react-router'

export const LoginLayout = () => {
  const { prefix } = useLoginLayoutStyle('login-layout')
  const navagate = useNavigate()

  const [layout] = useState<AdminLayoutInterface>({
    image: 'https://images.unsplash.com/photo-1522202176988-66273c2fd55f',
    title: 'Đăng nhập và hệ thống!'
  })

  const onFinish: FormProps<FieldType>['onFinish'] = (values) => {
    console.log('Login success:', values)
    navagate('/admin')
  }

  return (
    <div className={prefix}>
      <Row>
        <Col xs={0} md={15} className='left'>
          <Image src={layout.image} preview={false} />
        </Col>

        <Col xs={24} md={9} className='right'>
          <div className='content'>
            <h1>{layout.title}</h1>
            <Form
              layout='vertical'
              initialValues={{ remember: true }}
              onFinish={onFinish}
              labelCol={{
                span: 24
              }}
            >
              <Form.Item<FieldType>
                label='Tên đăng nhập'
                name='username'
                labelCol={{
                  span: 24
                }}
                labelAlign='left'
                rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}
              >
                <Input />
              </Form.Item>

              <Form.Item<FieldType>
                label='Mật khẩu'
                name='password'
                labelCol={{
                  span: 24
                }}
                labelAlign='left'
                rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}
              >
                <Input.Password />
              </Form.Item>
              <Form.Item<FieldType> name='remember' valuePropName='checked' label={null}>
                <Checkbox>Ghi nhớ tài khoản</Checkbox>
                <Link style={{ float: 'right' }} to='/quen-mat-khau.html'>
                  Quên mật khẩu?
                </Link>
              </Form.Item>

              <Form.Item label={null}>
                <Button
                  type='primary'
                  htmlType='submit'
                  style={{
                    width: '100%'
                  }}
                >
                  Đăng nhập
                </Button>
              </Form.Item>
            </Form>
            <Divider plain>Hoặc</Divider>
            <Row>
              <Col span={24} style={{ textAlign: 'center' }}>
                Bạn chưa có tài khoản?{' '}
                <Link to='/dang-ky.html'>
                  <Button type='link' style={{ padding: 0 }}>
                    Đăng ký
                  </Button>
                </Link>
              </Col>
            </Row>
          </div>
          <div className='footer'>
            <footer>
              <p>&copy; 2026 Tên Công Ty. All rights reserved.</p>
              <a href='lien-he.html'>Liên hệ</a> | <a href='chinh-sach-bao-mat.html'>Chính sách bảo mật</a>
            </footer>
          </div>
        </Col>
      </Row>
    </div>
  )
}
