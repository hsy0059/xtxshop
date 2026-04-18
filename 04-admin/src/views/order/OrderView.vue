<template>
  <div class="order-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>订单管理</span>
        </div>
      </template>

      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item label="订单状态">
          <el-select v-model="searchForm.orderState" placeholder="请选择状态" clearable>
            <el-option label="待付款" :value="1" />
            <el-option label="待发货" :value="2" />
            <el-option label="待收货" :value="3" />
            <el-option label="待评价" :value="4" />
            <el-option label="已完成" :value="5" />
            <el-option label="已取消" :value="6" />
          </el-select>
        </el-form-item>
        <el-form-item label="搜索">
          <el-input v-model="searchForm.keyword" placeholder="订单号/收货人/手机号" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <el-table :data="orderList" v-loading="loading" border>
        <el-table-column type="index" width="50" />
        <el-table-column prop="id" label="订单编号" min-width="180" show-overflow-tooltip />
        <el-table-column label="商品信息" min-width="300">
          <template #default="{ row }">
            <div v-for="item in row.items" :key="item.id" class="order-item">
              <el-image :src="item.image" style="width: 50px; height: 50px" fit="cover" />
              <div class="item-info">
                <div class="item-name">{{ item.name }}</div>
                <div class="item-attrs">{{ item.attrsText }}</div>
                <div class="item-price">¥{{ item.curPrice }} x {{ item.quantity }}</div>
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column label="订单金额" width="120">
          <template #default="{ row }">
            <div style="color: #f56c6c; font-weight: bold">¥{{ row.payMoney }}</div>
            <div style="font-size: 12px; color: #999">运费: ¥{{ row.postFee }}</div>
          </template>
        </el-table-column>
        <el-table-column label="收货信息" min-width="200">
          <template #default="{ row }">
            <div>{{ row.receiverContact }} {{ row.receiverMobile }}</div>
            <div style="font-size: 12px; color: #666">{{ row.receiverAddress }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="orderState" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="getOrderStateType(row.orderState)">
              {{ getOrderStateText(row.orderState) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="下单时间" width="180" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button v-if="row.orderState === 2" type="primary" link @click="handleShip(row)">发货</el-button>
            <el-button type="primary" link @click="handleDetail(row)">详情</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="pagination">
        <el-pagination
          v-model:current-page="page"
          v-model:page-size="pageSize"
          :total="total"
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-card>

    <el-dialog v-model="shipDialogVisible" title="订单发货" width="500px">
      <el-form ref="shipFormRef" :model="shipForm" :rules="shipRules" label-width="100px">
        <el-form-item label="物流公司" prop="logisticsCompany">
          <el-select v-model="shipForm.logisticsCompany" placeholder="请选择物流公司">
            <el-option label="顺丰速运" value="顺丰速运" />
            <el-option label="中通快递" value="中通快递" />
            <el-option label="圆通速递" value="圆通速递" />
            <el-option label="韵达快递" value="韵达快递" />
            <el-option label="申通快递" value="申通快递" />
          </el-select>
        </el-form-item>
        <el-form-item label="物流单号" prop="logisticsNo">
          <el-input v-model="shipForm.logisticsNo" placeholder="请输入物流单号" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="shipDialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="shipLoading" @click="handleShipSubmit">确定</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="detailDialogVisible" title="订单详情" width="700px">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="订单编号">{{ currentOrder?.id }}</el-descriptions-item>
        <el-descriptions-item label="订单状态">
          <el-tag :type="getOrderStateType(currentOrder?.orderState)">
            {{ getOrderStateText(currentOrder?.orderState) }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="下单时间">{{ currentOrder?.createTime }}</el-descriptions-item>
        <el-descriptions-item label="支付时间">{{ currentOrder?.payTime || '-' }}</el-descriptions-item>
        <el-descriptions-item label="发货时间">{{ currentOrder?.shipTime || '-' }}</el-descriptions-item>
        <el-descriptions-item label="收货时间">{{ currentOrder?.receiveTime || '-' }}</el-descriptions-item>
        <el-descriptions-item label="收货人">{{ currentOrder?.receiverContact }}</el-descriptions-item>
        <el-descriptions-item label="联系电话">{{ currentOrder?.receiverMobile }}</el-descriptions-item>
        <el-descriptions-item label="收货地址" :span="2">{{ currentOrder?.receiverAddress }}</el-descriptions-item>
        <el-descriptions-item label="商品总价">¥{{ currentOrder?.totalMoney }}</el-descriptions-item>
        <el-descriptions-item label="运费">¥{{ currentOrder?.postFee }}</el-descriptions-item>
        <el-descriptions-item label="实付金额" :span="2">
          <span style="color: #f56c6c; font-size: 18px; font-weight: bold">¥{{ currentOrder?.payMoney }}</span>
        </el-descriptions-item>
      </el-descriptions>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { getOrderList, shipOrder } from '@/api/order'
import type { Order } from '@/types'

const loading = ref(false)
const orderList = ref<Order[]>([])
const page = ref(1)
const pageSize = ref(10)
const total = ref(0)

const searchForm = reactive({
  orderState: undefined as number | undefined,
  keyword: ''
})

const shipDialogVisible = ref(false)
const shipLoading = ref(false)
const shipFormRef = ref()
const currentOrderId = ref('')

const shipForm = reactive({
  logisticsCompany: '',
  logisticsNo: ''
})

const shipRules = {
  logisticsCompany: [{ required: true, message: '请选择物流公司', trigger: 'change' }],
  logisticsNo: [{ required: true, message: '请输入物流单号', trigger: 'blur' }]
}

const detailDialogVisible = ref(false)
const currentOrder = ref<Order | null>(null)

const getOrderStateText = (state?: number) => {
  const map: Record<number, string> = {
    1: '待付款',
    2: '待发货',
    3: '待收货',
    4: '待评价',
    5: '已完成',
    6: '已取消'
  }
  return map[state || 0] || '未知'
}

const getOrderStateType = (state?: number) => {
  const map: Record<number, any> = {
    1: 'warning',
    2: 'primary',
    3: 'success',
    4: 'info',
    5: 'success',
    6: 'info'
  }
  return map[state || 0] || 'info'
}

const fetchOrderList = async () => {
  loading.value = true
  try {
    const result = await getOrderList({
      page: page.value,
      pageSize: pageSize.value,
      orderState: searchForm.orderState,
      keyword: searchForm.keyword
    })
    orderList.value = result.list
    total.value = result.total
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const handleSearch = () => {
  page.value = 1
  fetchOrderList()
}

const handleReset = () => {
  searchForm.orderState = undefined
  searchForm.keyword = ''
  page.value = 1
  fetchOrderList()
}

const handleShip = (row: Order) => {
  currentOrderId.value = row.id
  shipForm.logisticsCompany = ''
  shipForm.logisticsNo = ''
  shipDialogVisible.value = true
}

const handleShipSubmit = async () => {
  const valid = await shipFormRef.value?.validate()
  if (!valid) return

  shipLoading.value = true
  try {
    await shipOrder(currentOrderId.value, shipForm)
    ElMessage.success('发货成功')
    shipDialogVisible.value = false
    fetchOrderList()
  } catch (error) {
    console.error(error)
  } finally {
    shipLoading.value = false
  }
}

const handleDetail = (row: Order) => {
  currentOrder.value = row
  detailDialogVisible.value = true
}

const handleSizeChange = (val: number) => {
  pageSize.value = val
  fetchOrderList()
}

const handleCurrentChange = (val: number) => {
  page.value = val
  fetchOrderList()
}

onMounted(() => {
  fetchOrderList()
})
</script>

<style scoped lang="scss">
.order-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .search-form {
    margin-bottom: 20px;
  }

  .order-item {
    display: flex;
    align-items: center;
    padding: 10px 0;
    border-bottom: 1px solid #ebeef5;

    &:last-child {
      border-bottom: none;
    }

    .item-info {
      margin-left: 10px;
      flex: 1;

      .item-name {
        font-size: 14px;
        color: #303133;
      }

      .item-attrs {
        font-size: 12px;
        color: #909399;
        margin-top: 5px;
      }

      .item-price {
        font-size: 12px;
        color: #f56c6c;
        margin-top: 5px;
      }
    }
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>
