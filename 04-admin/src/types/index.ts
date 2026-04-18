export interface ApiResponse<T> {
  code: number
  msg: string
  result: T
}

export interface AdminLoginResult {
  token: string
  id: string
  username: string
  nickname?: string
  avatar?: string
}

export interface AdminUserInfo {
  id: string
  username: string
  nickname?: string
  avatar?: string
  email?: string
  status: number
  role: number
  createdAt: string
  lastLoginAt?: string
}

export interface DashboardStats {
  totalUsers: number
  totalOrders: number
  totalGoods: number
  totalSales: number
  orderStats: OrderStatsItem[]
  salesTrend: SalesTrendItem[]
}

export interface OrderStatsItem {
  state: number
  stateText: string
  count: number
}

export interface SalesTrendItem {
  date: string
  amount: number
}

export interface Goods {
  id: string
  name: string
  desc?: string
  price: number
  oldPrice: number
  mainPicture?: string
  inventory: number
  salesCount: number
  status: number
  categoryId?: string
  categoryName?: string
  createdAt: string
}

export interface Category {
  id: string
  name: string
  icon?: string
  picture?: string
  sort: number
  level: number
  parentId?: string
  children?: Category[]
}

export interface Order {
  id: string
  orderState: number
  orderStateText: string
  receiverContact: string
  receiverMobile: string
  receiverAddress: string
  totalMoney: number
  postFee: number
  payMoney: number
  createTime: string
  payTime?: string
  shipTime?: string
  receiveTime?: string
  items: OrderItem[]
}

export interface OrderItem {
  id: string
  name?: string
  image?: string
  quantity: number
  curPrice: number
  attrsText?: string
}

export interface User {
  id: string
  account: string
  nickname?: string
  avatar?: string
  mobile: string
  gender?: string
  birthday?: string
  fullLocation?: string
  profession?: string
  createdAt: string
}

export interface Banner {
  id: string
  imgUrl: string
  hrefUrl?: string
  type: number
  sort: number
  status: number
}

export interface HotRecommend {
  id: string
  title: string
  alt?: string
  target?: string
  pictures: string[]
  type: string
  sort: number
  status: number
}
