<template>
  <div class="dashboard">
    <el-row :gutter="20">
      <el-col :span="6">
        <el-card>
          <div class="stat-item">
            <div class="stat-icon" style="background: #409eff">
              <el-icon><User /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-value">{{ stats.totalUsers }}</div>
              <div class="stat-label">总用户数</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card>
          <div class="stat-item">
            <div class="stat-icon" style="background: #67c23a">
              <el-icon><Goods /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-value">{{ stats.totalGoods }}</div>
              <div class="stat-label">商品总数</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card>
          <div class="stat-item">
            <div class="stat-icon" style="background: #e6a23c">
              <el-icon><List /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-value">{{ stats.totalOrders }}</div>
              <div class="stat-label">订单总数</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card>
          <div class="stat-item">
            <div class="stat-icon" style="background: #f56c6c">
              <el-icon><Money /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-value">¥{{ stats.totalSales.toFixed(2) }}</div>
              <div class="stat-label">总销售额</div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="chart-row">
      <el-col :span="12">
        <el-card>
          <template #header>
            <span>订单状态分布</span>
          </template>
          <div ref="orderChartRef" style="height: 300px"></div>
        </el-card>
      </el-col>
      <el-col :span="12">
        <el-card>
          <template #header>
            <span>销售趋势（近7天）</span>
          </template>
          <div ref="salesChartRef" style="height: 300px"></div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick } from 'vue'
import * as echarts from 'echarts'
import { getDashboardStats } from '@/api/dashboard'
import type { DashboardStats } from '@/types'

const stats = ref<DashboardStats>({
  totalUsers: 0,
  totalOrders: 0,
  totalGoods: 0,
  totalSales: 0,
  orderStats: [],
  salesTrend: []
})

const orderChartRef = ref<HTMLElement>()
const salesChartRef = ref<HTMLElement>()

const fetchStats = async () => {
  try {
    const result = await getDashboardStats()
    stats.value = result
    initCharts()
  } catch (error) {
    console.error(error)
  }
}

const initCharts = () => {
  nextTick(() => {
    if (orderChartRef.value) {
      const orderChart = echarts.init(orderChartRef.value)
      orderChart.setOption({
        tooltip: { trigger: 'item' },
        legend: { bottom: '5%' },
        series: [
          {
            type: 'pie',
            radius: ['40%', '70%'],
            avoidLabelOverlap: false,
            itemStyle: {
              borderRadius: 10,
              borderColor: '#fff',
              borderWidth: 2
            },
            label: { show: false },
            data: stats.value.orderStats.map(item => ({
              name: item.stateText,
              value: item.count
            }))
          }
        ]
      })
    }

    if (salesChartRef.value) {
      const salesChart = echarts.init(salesChartRef.value)
      salesChart.setOption({
        tooltip: { trigger: 'axis' },
        xAxis: {
          type: 'category',
          data: stats.value.salesTrend.map(item => item.date)
        },
        yAxis: { type: 'value' },
        series: [
          {
            data: stats.value.salesTrend.map(item => item.amount),
            type: 'line',
            smooth: true,
            areaStyle: {
              color: {
                type: 'linear',
                x: 0, y: 0, x2: 0, y2: 1,
                colorStops: [
                  { offset: 0, color: 'rgba(64, 158, 255, 0.3)' },
                  { offset: 1, color: 'rgba(64, 158, 255, 0.05)' }
                ]
              }
            },
            itemStyle: { color: '#409eff' }
          }
        ]
      })
    }
  })
}

onMounted(() => {
  fetchStats()
})
</script>

<style scoped lang="scss">
.dashboard {
  .stat-item {
    display: flex;
    align-items: center;

    .stat-icon {
      width: 60px;
      height: 60px;
      border-radius: 8px;
      display: flex;
      align-items: center;
      justify-content: center;
      color: #fff;
      font-size: 28px;
      margin-right: 15px;
    }

    .stat-info {
      .stat-value {
        font-size: 24px;
        font-weight: bold;
        color: #303133;
      }

      .stat-label {
        font-size: 14px;
        color: #909399;
        margin-top: 5px;
      }
    }
  }

  .chart-row {
    margin-top: 20px;
  }
}
</style>
